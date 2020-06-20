using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

public class Alert : State
{
    private const string stateName = "Alert";
    private const bool isStunState = false;
    public override string Name
    {
        get
        {
            return stateName;
        }
    }
    public override bool IsStunState
    {
        get
        {
            return isStunState;
        }
    }

    public float movementSpeed;
    public float visionRange;
    public float fovDegrees;
    public float peripheralRange;
    public LayerMask visionMask;
    public LayerMask lineOfSightMask;

    Vector3 lastSeen;

    public override void Entry(params object[] data)
    {
        Debug.Log("Alert Start()");
        AiAgent.GetComponent<NavMeshAgent>().speed = movementSpeed;

        AiAgent.Lights[0].range = peripheralRange;
        AiAgent.Lights[0].color = Color.yellow;
        AiAgent.Lights[1].range = visionRange;
        AiAgent.Lights[1].spotAngle = fovDegrees;
        AiAgent.Lights[1].color = Color.yellow;

        lastSeen = (Vector3)data[0];
        AiAgent.GetComponent<NavMeshAgent>().SetDestination(lastSeen);
        base.Entry(data);
    }
    public override IEnumerator Step()
    {
        while (!Exiting)
        {
            if (!VisualCheck())
            {
                if (!AiAgent.GetComponent<NavMeshAgent>().hasPath)
                {
                    AiAgent.ChangeState(linkedStateNames[0]);
                }
            }

            EndOfFrameYield = new WaitForEndOfFrame();
            yield return EndOfFrameYield;
        }
        Exiting = false;
        yield return null;
    }
    public override void DebugGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(lastSeen, 0.2f);
        Gizmos.DrawRay(AiAgent.transform.position, lastSeen - AiAgent.transform.position);
    }
    
    bool VisualCheck()
    {
        Vector3 currentPosition = AiAgent.transform.position;
        Collider[] sensedColliders = Physics.OverlapSphere(currentPosition, visionRange, visionMask);
        if (sensedColliders.Length != 0)
        {
            Vector3 sensedPosition = sensedColliders[0].transform.position;
            bool inFieldOfView = Vector3.Angle(AiAgent.transform.forward, sensedPosition - currentPosition) < fovDegrees / 2 || Physics.OverlapSphere(currentPosition, peripheralRange, visionMask).Length != 0;
            if (inFieldOfView)
            {
                bool inLineOfSight = !Physics.Raycast(currentPosition, sensedPosition - currentPosition, Vector3.Distance(currentPosition, sensedPosition), lineOfSightMask);
                if (inLineOfSight)
                {
                    AiAgent.ChangeState(linkedStateNames[1], sensedColliders[0].transform);
                    return true;
                }
            }
        }
        return false;
    }
}
