using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

public class Hostile : State
{
    private const string stateName = "Hostile";
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

    Transform target;

    public override void Entry(params object[] data)
    {
        Debug.Log("Hostile Start()");
        AiAgent.GetComponent<NavMeshAgent>().speed = movementSpeed;

        AiAgent.Lights[0].range = peripheralRange;
        AiAgent.Lights[0].color = Color.red;

        AiAgent.Lights[1].range = visionRange;
        AiAgent.Lights[1].spotAngle = fovDegrees;
        AiAgent.Lights[1].color = Color.red;

        target = data[0] as Transform;
        base.Entry();
    }
    public override IEnumerator Step()
    {
        while (!Exiting)
        {
            if (!VisualCheck())
            {
                AiAgent.GetComponent<NavMeshAgent>().SetDestination(target.position);
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
        Gizmos.DrawSphere(target.position, 0.2f);
        Gizmos.DrawRay(AiAgent.transform.position, target.position - AiAgent.transform.position);
    }
    
    bool VisualCheck()
    {
        Vector3 currentPosition = AiAgent.transform.position;
        bool inLineofSight = !Physics.Raycast(currentPosition, target.position - currentPosition, Vector3.Distance(currentPosition, target.position), lineOfSightMask);
        bool inFieldOfView = Vector3.Angle(AiAgent.transform.forward, target.position - currentPosition) < fovDegrees / 2 || Physics.OverlapSphere(currentPosition, peripheralRange, visionMask).Length != 0;
        if ((!inLineofSight && inFieldOfView) || !inFieldOfView)
        {
            AiAgent.ChangeState(linkedStateNames[0], target.position);
            return true;
        }
        return false;
    }
}
