using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    private const string name = "Idle";
    private const bool isStunState = true;
    public override string Name
    {
        get
        {
            return name;
        }
    }
    public override bool IsStunState
    {
        get
        {
            return isStunState;
        }
    }

    float movementSpeed;
    float visionRange;
    float fovDegrees;
    float peripheralRange;
    LayerMask visionMask;
    LayerMask lineOfSightMask;
    Transform path;

    int currentWaypoint = 0;
    bool goingForward = false;

    float stunTime = 0f;
    bool stunned = false;

    //Debug
    Vector3[] lineOfSight = new Vector3[2];
    //End Debug

    public Idle(GameObject agent, StateDesc stateDesc) : base(agent)
    {
        string[] parameters = new string[10]
        {
            GetParameterFromName("movementSpeed", stateDesc.stateParameters),
            GetParameterFromName("visionMask", stateDesc.stateParameters),
            GetParameterFromName("visionRange", stateDesc.stateParameters),
            GetParameterFromName("fovDegrees", stateDesc.stateParameters),
            GetParameterFromName("peripheralRange", stateDesc.stateParameters),
            GetParameterFromName("linkedStates", stateDesc.stateParameters),
            GetParameterFromName("path", stateDesc.stateParameters),
            null,
            null,
            null
        };
        movementSpeed    = parameters[0] != null ? Convert.ToSingle(parameters[0], CultureInfo.InvariantCulture) : 5f;
        visionMask       = parameters[1] != null ? LayerMask.GetMask(parameters[1]) : 1 << 0;
        visionRange      = parameters[2] != null ? Convert.ToSingle(parameters[2], CultureInfo.InvariantCulture) : 10f;
        fovDegrees       = parameters[3] != null ? Convert.ToSingle(parameters[3], CultureInfo.InvariantCulture) / 2 : 45f;
        peripheralRange  = parameters[4] != null ? Convert.ToSingle(parameters[4], CultureInfo.InvariantCulture) : 1f;
        LinkedStateNames = parameters[5] != null ? parameters[5].Split(null) : new string[1] { "Hostile" };
        lineOfSightMask  = parameters[7] != null ? LayerMask.GetMask(parameters[6]) : LayerMask.GetMask("VisualObstacle");
        path             = GameObject.Find(parameters[6]).transform;
    }
    public override void Start(params object[] data)
    {
        Debug.Log("Idle Start()");
        AiAgent.GetComponent<NavMeshAgent>().speed = movementSpeed;

        AiAgent.Lights[0].range = peripheralRange;
        AiAgent.Lights[0].color = Color.white;
        AiAgent.Lights[1].range = visionRange;
        AiAgent.Lights[1].spotAngle = fovDegrees * 2;
        AiAgent.Lights[1].color = Color.white;

        FindNearWaypoint();
        AiAgent.GetComponent<NavMeshAgent>().SetDestination(path.GetChild(currentWaypoint).position);
    }
    public override void Update()
    {
        if(!VisualCheck())
        {
            PathFollowHandling();
        }   
    }
    public override void End()
    {
        Debug.Log("Idle End()");
        lineOfSight = new Vector3[2];
    }
    public override void Stun(float time)
    {
        stunTime = 5f;
        if (!stunned) { AiAgent.StartCoroutine(Unstun(time)); }
        stunned = true;
    }
    public override void DebugGizmos()
    {
        if (lineOfSight[0] != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawRay(lineOfSight[0], lineOfSight[1]);
        }
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
    IEnumerator Unstun(float time)
    {
        foreach (Light light in AiAgent.Lights) { light.color = Color.red + Color.blue; }
        AiAgent.GetComponent<NavMeshAgent>().isStopped = true;

        while (stunTime > 0)
        { 
            stunTime -= Time.deltaTime;
            yield return null;
        }

        foreach (Light light in AiAgent.Lights) { light.color = Color.white; }
        AiAgent.GetComponent<NavMeshAgent>().isStopped = false;

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

            bool inFieldOfView = Vector3.Angle(AiAgent.transform.forward, sensedPosition - currentPosition) < fovDegrees || Physics.OverlapSphere(currentPosition, peripheralRange, visionMask).Length != 0;
            if (inFieldOfView)
            {
                bool inLineOfSight = !Physics.Raycast(currentPosition, sensedPosition - currentPosition, Vector3.Distance(currentPosition, sensedPosition), lineOfSightMask);
                if(inLineOfSight)
                {
                    AiAgent.ChangeState(LinkedStateNames[0], sensedColliders[0].transform);
                    return true;
                }
            }
        }
        return false;
    }
}
