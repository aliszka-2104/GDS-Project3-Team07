using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    private const string stateName = "Idle";
    private const bool isStunState = true;
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
    public Transform path;

    int currentWaypoint = 0;
    bool goingForward = false;

    public float stunTime = 0f;
    bool stunned = false;


    //Debug
    Vector3[] lineOfSight = new Vector3[2];
    //End Debug

    public override void Entry(params object[] data)
    {
        Debug.Log("Idle Start()");
        AiAgent.GetComponent<NavMeshAgent>().speed = movementSpeed;

        AiAgent.Lights[0].range = peripheralRange;
        AiAgent.Lights[0].color = Color.white;
        AiAgent.Lights[1].range = visionRange;
        AiAgent.Lights[1].spotAngle = fovDegrees;
        AiAgent.Lights[1].color = Color.white;

        FindNearWaypoint();
        AiAgent.GetComponent<NavMeshAgent>().SetDestination(path.GetChild(currentWaypoint).position);
        base.Entry(data);
    }
    public override IEnumerator Step()
    {
        while(!Exiting)
        {
            if(!stunned)
            {
                if (!VisualCheck())
                {
                    PathFollowHandling();
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
        if (lineOfSight[0] != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawRay(lineOfSight[0], lineOfSight[1]);
        }
    }
    public override void Stun(float time)
    {
        stunTime = time;
        if (!stunned) { AiAgent.StartCoroutine(Unstun()); }
        stunned = true;
    }

    void FindNearWaypoint()
    {
        for (int i = 0; i < path.childCount; i++)
        {
            if (Vector3.Distance(AiAgent.transform.position, path.GetChild(i).position) <
                Vector3.Distance(AiAgent.transform.position, path.GetChild(currentWaypoint).position))
            {
                currentWaypoint = i;
            }
        }
    }
    void PathFollowHandling()
    {
        if(Vector3.Distance(AiAgent.transform.position, path.GetChild(currentWaypoint).position) < 0.1f)
        {
            if(currentWaypoint == 0 || currentWaypoint == path.childCount-1)
            {
                goingForward = !goingForward;
                currentWaypoint -= Convert.ToSingle(currentWaypoint).CompareTo(0.5f);
            }
            else if (goingForward)
            {
                currentWaypoint += 1;
            }
            else
            {
                currentWaypoint -= 1;
            }
            AiAgent.GetComponent<NavMeshAgent>().SetDestination(path.GetChild(currentWaypoint).position);
        }
    }
    IEnumerator Unstun()
    {
        foreach (Light light in AiAgent.Lights) { light.color = Color.red + Color.blue; }
        AiAgent.GetComponent<NavMeshAgent>().enabled = false;

        while (stunTime > 0)
        { 
            stunTime -= Time.deltaTime;
            yield return null;
        }

        foreach (Light light in AiAgent.Lights) { light.color = Color.white; }
        AiAgent.GetComponent<NavMeshAgent>().enabled = true;
        FindNearWaypoint();
        AiAgent.GetComponent<NavMeshAgent>().SetDestination(path.GetChild(currentWaypoint).position);


        stunned = false;
        yield return null;
    }
    bool VisualCheck()
    {
        if(stunned) { return false; }
        Vector3 currentPosition = AiAgent.transform.position;
        Collider[] sensedColliders = Physics.OverlapSphere(currentPosition, visionRange, visionMask);
        if (sensedColliders.Length != 0)
        {
            Vector3 sensedPosition = sensedColliders[0].transform.position;

            //Debug
            if (AiAgent.debug)
            {
                lineOfSight[0] = currentPosition;
                lineOfSight[1] = sensedPosition - currentPosition;
            }
            //End Debug

            bool inFieldOfView = Vector3.Angle(AiAgent.transform.forward, sensedPosition - currentPosition) < fovDegrees / 2 || Physics.OverlapSphere(currentPosition, peripheralRange, visionMask).Length != 0;
            if (inFieldOfView)
            {
                bool inLineOfSight = !Physics.Raycast(currentPosition, sensedPosition - currentPosition, Vector3.Distance(currentPosition, sensedPosition), lineOfSightMask);
                if(inLineOfSight)
                {
                    AiAgent.ChangeState(linkedStateNames[0], sensedColliders[0].transform);
                    return true;
                }
            }
        }
        return false;
    }
}
