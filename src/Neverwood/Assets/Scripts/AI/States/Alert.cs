using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Alert : State
{
    private const string name = "Alert";
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
    LayerMask lineOfSightMask = LayerMask.GetMask("VisualObstacle");

    Vector3 lastSeen;
    Transform lights;

    public Alert(GameObject agent, StateDesc stateDesc) : base(agent)
    {
        string[] parameters = new string[6]
        {
            GetParameterFromName("movementSpeed", stateDesc.stateParameters),
            GetParameterFromName("visionMask", stateDesc.stateParameters),
            GetParameterFromName("visionRange", stateDesc.stateParameters),
            GetParameterFromName("fovDegrees", stateDesc.stateParameters),
            GetParameterFromName("peripheralRange", stateDesc.stateParameters),
            GetParameterFromName("linkedStates", stateDesc.stateParameters)
        };
        movementSpeed    = parameters[0] != null ? Convert.ToSingle(parameters[0], CultureInfo.InvariantCulture) : 5f;
        visionMask       = parameters[1] != null ? LayerMask.GetMask(parameters[1]) : 1 << 0;
        visionRange      = parameters[2] != null ? Convert.ToSingle(parameters[2], CultureInfo.InvariantCulture) : 10f;
        fovDegrees       = parameters[3] != null ? Convert.ToSingle(parameters[3], CultureInfo.InvariantCulture) / 2 : 45f;
        peripheralRange  = parameters[4] != null ? Convert.ToSingle(parameters[4], CultureInfo.InvariantCulture) : 1f;
        LinkedStateNames = parameters[5] != null ? parameters[5].Split(null) : new string[2] { "Idle", "Hostile" };

        lights = AiGameObject.transform.Find("Light");
    }

    public override void Start(params object[] data)
    {
        Debug.Log("Alert Start()");
        AiGameObject.GetComponent<NavMeshAgent>().speed = movementSpeed;

        lights.Find("Peripheral").GetComponent<Light>().range = peripheralRange;
        lights.Find("Peripheral").GetComponent<Light>().color = new Color(1, 1, 0, 1);
        lights.Find("FoV").GetComponent<Light>().range = visionRange;
        lights.Find("FoV").GetComponent<Light>().spotAngle = fovDegrees * 2;
        lights.Find("FoV").GetComponent<Light>().color = new Color(1, 1, 0, 1);

        lastSeen = (Vector3)data[0];
        AiGameObject.GetComponent<NavMeshAgent>().SetDestination(lastSeen);
    }
    public override void Update()
    {
        if(!VisualCheck())
        {
            if (!AiGameObject.GetComponent<NavMeshAgent>().hasPath)
            {
                AiGameObject.GetComponent<Agent>().ChangeState(LinkedStateNames[0]);
            }
        }
    }
    public override void End()
    {
        Debug.Log("Alert End()");
    }
    public override void DebugGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 1f);
        Transform currentPosition = AiGameObject.transform;
        Gizmos.DrawRay(currentPosition.position, currentPosition.forward * visionRange);
        Gizmos.DrawRay(currentPosition.position, Quaternion.Euler(0, fovDegrees, 0) * currentPosition.forward * visionRange);
        Gizmos.DrawRay(currentPosition.position, Quaternion.Euler(0, -fovDegrees, 0) * currentPosition.forward * visionRange);
        Gizmos.DrawWireSphere(currentPosition.position, peripheralRange);

        Gizmos.color = new Color(1, 1, 1, 1f);
        Gizmos.DrawSphere(lastSeen, 0.2f);
        Gizmos.DrawRay(currentPosition.position, lastSeen - currentPosition.position);
    }
    
    bool VisualCheck()
    {
        Vector3 currentPosition = AiGameObject.transform.position;
        Collider[] sensedColliders = Physics.OverlapSphere(currentPosition, visionRange, visionMask);
        if (sensedColliders.Length != 0)
        {
            Vector3 sensedPosition = sensedColliders[0].transform.position;
            bool inFieldOfView = Vector3.Angle(AiGameObject.transform.forward, sensedPosition - currentPosition) < fovDegrees || Physics.OverlapSphere(currentPosition, peripheralRange, visionMask).Length != 0;
            if (inFieldOfView)
            {
                bool inLineOfSight = !Physics.Raycast(currentPosition, sensedPosition - currentPosition, Vector3.Distance(currentPosition, sensedPosition), lineOfSightMask);
                if (inLineOfSight)
                {
                    AiGameObject.GetComponent<Agent>().ChangeState(LinkedStateNames[1]);
                    return true;
                }
            }
        }
        return false;
    }
}
