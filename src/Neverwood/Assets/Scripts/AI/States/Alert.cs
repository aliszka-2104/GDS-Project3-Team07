using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

public class Alert : State
{
    private const string name = "Alert";
    private const bool isStunState = false;
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

    Vector3 lastSeen;

    public Alert(GameObject agent, StateDesc stateDesc) : base(agent)
    {
        string[] parameters = new string[10]
        {
            GetParameterFromName("movementSpeed", stateDesc.stateParameters),
            GetParameterFromName("visionMask", stateDesc.stateParameters),
            GetParameterFromName("visionRange", stateDesc.stateParameters),
            GetParameterFromName("fovDegrees", stateDesc.stateParameters),
            GetParameterFromName("peripheralRange", stateDesc.stateParameters),
            GetParameterFromName("linkedStates", stateDesc.stateParameters),
            null,
            null,
            null,
            null
        };
        movementSpeed    = parameters[0] != null ? Convert.ToSingle(parameters[0], CultureInfo.InvariantCulture) : 5f;
        visionMask       = parameters[1] != null ? LayerMask.GetMask(parameters[1]) : 1 << 0;
        visionRange      = parameters[2] != null ? Convert.ToSingle(parameters[2], CultureInfo.InvariantCulture) : 10f;
        fovDegrees       = parameters[3] != null ? Convert.ToSingle(parameters[3], CultureInfo.InvariantCulture) / 2 : 45f;
        peripheralRange  = parameters[4] != null ? Convert.ToSingle(parameters[4], CultureInfo.InvariantCulture) : 1f;
        LinkedStateNames = parameters[5] != null ? parameters[5].Split(null) : new string[2] { "Idle", "Hostile" };
        lineOfSightMask =  parameters[6] != null ? LayerMask.GetMask(parameters[6]) : LayerMask.GetMask("VisualObstacle");
    }

    public override void Start(params object[] data)
    {
        Debug.Log("Alert Start()");
        AiAgent.GetComponent<NavMeshAgent>().speed = movementSpeed;

        AiAgent.Lights[0].range = peripheralRange;
        AiAgent.Lights[0].color = Color.yellow;
        AiAgent.Lights[1].range = visionRange;
        AiAgent.Lights[1].spotAngle = fovDegrees * 2;
        AiAgent.Lights[1].color = Color.yellow;

        lastSeen = (Vector3)data[0];
        AiAgent.GetComponent<NavMeshAgent>().SetDestination(lastSeen);
    }
    public override void Update()
    {
        if(!VisualCheck())
        {
            if (!AiAgent.GetComponent<NavMeshAgent>().hasPath)
            {
                AiAgent.ChangeState(LinkedStateNames[0]);
            }
        }
    }
    public override void End()
    {
        Debug.Log("Alert End()");
        lastSeen = new Vector3();
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
            bool inFieldOfView = Vector3.Angle(AiAgent.transform.forward, sensedPosition - currentPosition) < fovDegrees || Physics.OverlapSphere(currentPosition, peripheralRange, visionMask).Length != 0;
            if (inFieldOfView)
            {
                bool inLineOfSight = !Physics.Raycast(currentPosition, sensedPosition - currentPosition, Vector3.Distance(currentPosition, sensedPosition), lineOfSightMask);
                if (inLineOfSight)
                {
                    AiAgent.ChangeState(LinkedStateNames[1], sensedColliders[0].transform);
                    return true;
                }
            }
        }
        return false;
    }
}
