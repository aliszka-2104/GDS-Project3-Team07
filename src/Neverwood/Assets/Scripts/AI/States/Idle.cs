﻿using System;
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
    public Splice splicePath;
    [Header("State Links")]
    public StateType onTargetSensedStateChange = StateType.Alert;

    #endregion
    #region Private value holders 

    WaitForSeconds waitFor;
    Collider target;
    Vector3 destination;
    private int currentWaypoint = 0;
    private bool goingForward = true;

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
        if (splicePath != null)
        {
            GoToWaypoint(splicePath.path[currentWaypoint]);
        }
    }
    public object[] Exit()
    {
        return new object[1] { target };
    }
    public IEnumerator StateProcess()
    {
        target = TargetSense();
        if (target != null)
        {
            GetComponent<Agent>().ChangeState(onTargetSensedStateChange);
        }
        if (splicePath != null)
        {
            HandlePathFollow();
        }
        yield return waitFor = new WaitForSeconds(1f / executesPerSecond);
    }

    #endregion
    #region Methods

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
        if (sensedTarget == null)
        {
            Collider[] auditoryPossibleTargets = GetComponent<Hearing>().UseSense();
            if (auditoryPossibleTargets.Length != 0)
            {
                sensedTarget = auditoryPossibleTargets[0];
            }
        }
        return sensedTarget;
    }
    void GoToWaypoint(Vector3 wp)
    {
        destination = new Vector3(wp.x, transform.position.y, wp.z);
        GetComponent<NavMeshAgent>().SetDestination(destination);
    }
    void HandlePathFollow()
    {
        if (Vector3.Distance(transform.position, destination) <= 0.1f)
        {
            if (currentWaypoint == splicePath.path.Length - 1)
            {
                goingForward = false;
            }
            else if (currentWaypoint == 0)
            {
                goingForward = true;
            }
            if (goingForward)
            {
                currentWaypoint++;
            }
            else
            {
                currentWaypoint--;
            }
            if (splicePath != null)
            {
                GoToWaypoint(splicePath.path[currentWaypoint]);
            }
        }
    }
    void Turn(float velocity)
    {
        transform.Rotate(0f, velocity * Time.deltaTime, 0f);
    }

    #endregion
}