using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows.WebCam;

[System.Serializable]
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
    LayerMask lineOfSightMask = LayerMask.GetMask("VisualObstacle");

    Vector3 lastSeen;
    Transform target;
    Transform lights;
    
    
    public Hostile(GameObject agent, StateDesc stateDesc) : base(agent)  // 0 - reserved, 1 - movementSpeed, 2 - LayerMask name, 3 - Vision Range, 4 - FoV Degrees, 5 - Peripheral Range
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
        LinkedStateNames = parameters[5] != null ? parameters[5].Split(null) : new string[1] { "Alert" };

        lights = AiGameObject.transform.Find("Light");
    }

    public override void Start(params object[] data)
    {
        Debug.Log("Hostile Start()");
        AiGameObject.GetComponent<NavMeshAgent>().speed = movementSpeed;

        lights.Find("Peripheral").GetComponent<Light>().range = peripheralRange;
        lights.Find("Peripheral").GetComponent<Light>().color = new Color(1, 0, 0, 1);
        lights.Find("FoV").GetComponent<Light>().range = visionRange;
        lights.Find("FoV").GetComponent<Light>().spotAngle = fovDegrees * 2;
        lights.Find("FoV").GetComponent<Light>().color = new Color(1, 0, 0, 1);

        Collider[] hitColliders = Physics.OverlapSphere(AiGameObject.transform.position, visionRange, visionMask);
        if (hitColliders.Length != 0)
        {
            target = hitColliders[0].transform;
        }
        lastSeen = target.position;
    }
    public override void Update()
    {
        if(!VisualCheck())
        {
            AiGameObject.GetComponent<NavMeshAgent>().SetDestination(target.position);
            lastSeen = target.position;
        }
    }
    public override void End()
    {
        Debug.Log("Hostile End()");
    }
    public override void DebugGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 1f);
        Transform currentPosition = AiGameObject.transform;
        Gizmos.DrawRay(currentPosition.position, currentPosition.forward * visionRange);
        Gizmos.DrawRay(currentPosition.position, Quaternion.Euler(0, fovDegrees, 0) * currentPosition.forward * visionRange);
        Gizmos.DrawRay(currentPosition.position, Quaternion.Euler(0, -fovDegrees, 0) * currentPosition.forward * visionRange);
        Gizmos.DrawWireSphere(currentPosition.position, peripheralRange);

        if (lastSeen != null)
        {
            Gizmos.color = new Color(1, 1, 1, 1);
            Gizmos.DrawSphere(lastSeen, 0.2f);
            Gizmos.DrawRay(currentPosition.position, lastSeen - currentPosition.position);
        }
    }

    bool VisualCheck()
    {
        Vector3 currentPosition = AiGameObject.transform.position;
        bool inLineofSight = !Physics.Raycast(currentPosition, target.position - currentPosition, Vector3.Distance(currentPosition, target.position), lineOfSightMask);
        bool inFieldOfView = Vector3.Angle(AiGameObject.transform.forward, target.position - currentPosition) < fovDegrees || Physics.OverlapSphere(currentPosition, peripheralRange, visionMask).Length != 0;
        if ((!inLineofSight && inFieldOfView) || !inFieldOfView)
        {
            AiGameObject.GetComponent<Agent>().ChangeState(LinkedStateNames[0], lastSeen);
            return true;
        }
        return false;
    }
}
