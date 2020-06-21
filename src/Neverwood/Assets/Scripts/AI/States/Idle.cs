using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Vision))]
[RequireComponent(typeof(Hearing))]
public class Idle : MonoBehaviour, IState
{
    #region State parameters

    [Header("Parameters")]
    public float movementSpeed = 1.5f;
    public float spotAngle = 45f;
    public float visionRange = 5f;
    public float peripheralVisionRange = 1f;
    public float hearingRange = 7f;
    public int executesPerSecond = 10;
    [Header("State Links")]
    public StateType onTargetSensedStateChange = StateType.Alert;

    #endregion
    #region Private value holders 

    WaitForSeconds waitFor;
    Collider target;

    #endregion
    #region Unity callbacks
    #endregion
    #region Interface implementation

    public string stateName { get; } = "Idle";
    public StateType stateType { get; } = StateType.Idle;
    public void Entry(object[] data = null)
    {
        GetComponent<NavMeshAgent>().speed = movementSpeed;
        GetComponent<Vision>().spotAngle = spotAngle;
        GetComponent<Vision>().range = visionRange;
        GetComponent<Vision>().peripheralVisionRange = peripheralVisionRange;
        GetComponent<Hearing>().range = hearingRange;

        target = null;
        GetComponent<NavMeshAgent>().ResetPath();
    }
    public object[] Exit()
    {
        return new object[1] { target };
    }
    public IEnumerator StateProcess()
    {
        target = TargetSense();
        if(target != null)
        {
            GetComponent<Agent>().ChangeState(onTargetSensedStateChange);
        }
        yield return waitFor = new WaitForSeconds(1f / executesPerSecond);
    }

    #endregion
    #region Methods

    Collider TargetSense()
    {
        Collider sensedTarget = null;
        Collider[] visualPossibleTargets = GetComponent<Vision>().UseSense();
        foreach(Collider coll in visualPossibleTargets)
        {
            if(coll.tag == "Player")
            {
                sensedTarget = coll;
                break;
            }
        }
        if(sensedTarget == null)
        {
            Collider[] auditoryPossibleTargets = GetComponent<Hearing>().UseSense();
            if(auditoryPossibleTargets.Length != 0)
            {
                sensedTarget = auditoryPossibleTargets[0];
            }
        }
        return sensedTarget;
    }

    #endregion
}