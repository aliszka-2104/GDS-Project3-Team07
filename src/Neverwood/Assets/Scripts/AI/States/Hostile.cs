using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Vision))]
public class Hostile : MonoBehaviour, IState
{
    #region State parameters

    [Header("Parameters")]
    public float movementSpeed = 2.5f;
    public float spotAngle = 60f;
    public float visionRange = 6f;
    public float peripheralVisionRange = 2f;
    public float hearingRange = 4f;
    public int executesPerSecond = 20;
    public float killRange = 1f;
    [Header("State Links")]
    public StateType onTargetLostStateChange = StateType.Alert;

    #endregion
    #region Private value holders 

    WaitForSeconds waitFor;
    Collider target;

    #endregion
    #region Unity callbacks
    #endregion
    #region Interface implementation

    public string stateName { get; } = "Hostile";
    public StateType stateType { get; } = StateType.Hostile;
    public void Entry(object[] data = null)
    {
        GetComponent<NavMeshAgent>().speed = movementSpeed;
        GetComponent<Vision>().spotAngle = spotAngle;
        GetComponent<Vision>().range = visionRange;
        GetComponent<Vision>().peripheralVisionRange = peripheralVisionRange;
        GetComponent<Hearing>().range = hearingRange;

        target = data[0] as Collider;
        SetDestinationToTarget();
    }
    public object[] Exit()
    {
        return new object[2] { target, true };
    }
    public IEnumerator StateProcess()
    {
        if (TargetSense() != target)
        {
            GetComponent<Agent>().ChangeState(onTargetLostStateChange);
        }
        else
        {
            KillRangeCheck();
            SetDestinationToTarget();
        }
        yield return waitFor = new WaitForSeconds(1f / executesPerSecond);
    }

    #endregion
    #region Methods

    void SetDestinationToTarget()
    {
        GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }
    Collider TargetSense()
    {
        Collider sensedTarget = null;
        Collider[] visualPossibleTargets = GetComponent<Vision>().UseSense();
        foreach (Collider coll in visualPossibleTargets)
        {
            if (coll.tag == "Player")
            {
                sensedTarget = coll;
                break;
            }
        }
        return sensedTarget;
    }
    void KillRangeCheck()
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= killRange)
        {
            if (target.tag == "Player")
            {
                SendMessage("OnAttackPlayer", target);
                //Application.Quit();
            }
        }
    }

    #endregion
}
