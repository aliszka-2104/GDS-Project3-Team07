using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

public class Hostile : State
{
    private const string name = "Hostile";
    public override string Name
    {
        get
        {
            return name;
        }
    }

    float movementSpeed;
    float visionRange;
    float fovDegrees;
    float peripheralRange;
    LayerMask visionMask;
    LayerMask lineOfSightMask;

    Transform target;
    
    
    public Hostile(GameObject agent, StateDesc stateDesc) : base(agent)  // 0 - reserved, 1 - movementSpeed, 2 - LayerMask name, 3 - Vision Range, 4 - FoV Degrees, 5 - Peripheral Range
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
        LinkedStateNames = parameters[5] != null ? parameters[5].Split(null) : new string[1] { "Alert" };
        lineOfSightMask  = parameters[6] != null ? LayerMask.GetMask(parameters[6]) : LayerMask.GetMask("VisualObstacle");
    }

    public override void Start(params object[] data)
    {
        Debug.Log("Hostile Start()");
        AiAgent.GetComponent<NavMeshAgent>().speed = movementSpeed;

        AiAgent.Lights[0].range = peripheralRange;
        AiAgent.Lights[0].color = Color.red;
        AiAgent.Lights[1].range = visionRange;
        AiAgent.Lights[1].spotAngle = fovDegrees * 2;
        AiAgent.Lights[1].color = Color.red;

        target = data[0] as Transform;
    }
    public override void Update()
    {
        if(!VisualCheck())
        {
            AiAgent.GetComponent<NavMeshAgent>().SetDestination(target.position);
        }
    }
    public override void End()
    {
        Debug.Log("Hostile End()");
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
        bool inFieldOfView = Vector3.Angle(AiAgent.transform.forward, target.position - currentPosition) < fovDegrees || Physics.OverlapSphere(currentPosition, peripheralRange, visionMask).Length != 0;
        if ((!inLineofSight && inFieldOfView) || !inFieldOfView)
        {
            AiAgent.ChangeState(LinkedStateNames[0], 0f, target.position);
            return true;
        }
        return false;
    }
}
