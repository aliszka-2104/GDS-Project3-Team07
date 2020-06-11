using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

[System.Serializable]
public class Idle : State
{
    private const string name = "Idle";
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

    Transform path;
    int currentWaypoint = 0;
    bool goingForward = false;
    Transform lights;

    //Debug
    Vector3[] lineOfSight = new Vector3[2];
    //End Debug

    public Idle(GameObject agent, StateDesc stateDesc) : base(agent)
    {
        string[] parameters = new string[7]
        {
            GetParameterFromName("movementSpeed", stateDesc.stateParameters),
            GetParameterFromName("visionMask", stateDesc.stateParameters),
            GetParameterFromName("visionRange", stateDesc.stateParameters),
            GetParameterFromName("fovDegrees", stateDesc.stateParameters),
            GetParameterFromName("peripheralRange", stateDesc.stateParameters),
            GetParameterFromName("linkedStates", stateDesc.stateParameters),
            GetParameterFromName("path", stateDesc.stateParameters)
        };
        movementSpeed    = parameters[0] != null ? Convert.ToSingle(parameters[0], CultureInfo.InvariantCulture) : 5f;
        visionMask       = parameters[1] != null ? LayerMask.GetMask(parameters[1]) : 1 << 0;
        visionRange      = parameters[2] != null ? Convert.ToSingle(parameters[2], CultureInfo.InvariantCulture) : 10f;
        fovDegrees       = parameters[3] != null ? Convert.ToSingle(parameters[3], CultureInfo.InvariantCulture) / 2 : 45f;
        peripheralRange  = parameters[4] != null ? Convert.ToSingle(parameters[4], CultureInfo.InvariantCulture) : 1f;
        LinkedStateNames = parameters[5] != null ? parameters[5].Split(null) : new string[1] { "Hostile" };
        path             = GameObject.Find(parameters[6]).transform;

        lights = AiGameObject.transform.Find("Light");
    }
    public override void Start(params object[] data)
    {
        Debug.Log("Idle Start()");
        AiGameObject.GetComponent<NavMeshAgent>().speed = movementSpeed;

        lights.Find("Peripheral").GetComponent<Light>().range = peripheralRange;
        lights.Find("Peripheral").GetComponent<Light>().color = new Color(1, 1, 1, 1);
        lights.Find("FoV").GetComponent<Light>().range = visionRange;
        lights.Find("FoV").GetComponent<Light>().spotAngle = fovDegrees * 2;
        lights.Find("FoV").GetComponent<Light>().color = new Color(1, 1, 1, 1);

        FindNearWaypoint();
        AiGameObject.GetComponent<NavMeshAgent>().SetDestination(path.GetChild(currentWaypoint).position);
    }
    public override void Update()
    {
        if(!VisualCheck()) { PathFollowHandling(); }   
    }
    public override void End()
    {
        Debug.Log("Idle End()");
        lineOfSight = new Vector3[2];
    }
    public override void DebugGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 1f);
        Transform currentPosition = AiGameObject.transform;
        Gizmos.DrawRay(currentPosition.position, currentPosition.forward * visionRange);
        Gizmos.DrawRay(currentPosition.position, Quaternion.Euler(0, fovDegrees, 0)  * currentPosition.forward * visionRange);
        Gizmos.DrawRay(currentPosition.position, Quaternion.Euler(0, -fovDegrees, 0) * currentPosition.forward * visionRange);
        Gizmos.DrawWireSphere(currentPosition.position, peripheralRange);

        if (lineOfSight[0] != null)
        {
            Gizmos.color = new Color(1, 1, 1, 1);
            Gizmos.DrawRay(lineOfSight[0], lineOfSight[1]);
        }
    }

    void FindNearWaypoint()
    {
        for (int i = 0; i < path.childCount; i++)
        {
            if (Vector3.Distance(AiGameObject.transform.position, path.GetChild(i).position) <
               Vector3.Distance(AiGameObject.transform.position, path.GetChild(currentWaypoint).position))
            {
                currentWaypoint = i;
            }
        }
    }
    void PathFollowHandling()
    {
        if(Vector3.Distance(AiGameObject.transform.position, path.GetChild(currentWaypoint).position) < 0.25f)
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
            AiGameObject.GetComponent<NavMeshAgent>().SetDestination(path.GetChild(currentWaypoint).position);
        }
    }
    bool VisualCheck()
    {
        Vector3 currentPosition = AiGameObject.transform.position;
        Collider[] sensedColliders = Physics.OverlapSphere(currentPosition, visionRange, visionMask);
        if (sensedColliders.Length != 0)
        {
            Vector3 sensedPosition = sensedColliders[0].transform.position;

            //Debug
            if (AiGameObject.GetComponent<Agent>().debug)
            {
                lineOfSight[0] = currentPosition;
                lineOfSight[1] = sensedPosition - currentPosition;
            }
            //End Debug

            bool inFieldOfView = Vector3.Angle(AiGameObject.transform.forward, sensedPosition - currentPosition) < fovDegrees || Physics.OverlapSphere(currentPosition, peripheralRange, visionMask).Length != 0;
            if (inFieldOfView)
            {
                bool inLineOfSight = !Physics.Raycast(currentPosition, sensedPosition - currentPosition, Vector3.Distance(currentPosition, sensedPosition), lineOfSightMask);
                if(inLineOfSight)
                {
                    AiGameObject.GetComponent<Agent>().ChangeState(LinkedStateNames[0]);
                    return true;
                }
            }
        }
        return false;
    }
}
