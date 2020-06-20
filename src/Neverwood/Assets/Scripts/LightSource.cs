using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightSource : MonoBehaviour
{
    public string affectedEntitiesTag;
    public float afflictionDurationSeconds;
    public bool lightEnabled;

    Collider triggerCollider;

    private void Awake()
    {
        switch(GetComponent<Light>().type)
        {
            case LightType.Point:
                {
                    triggerCollider = gameObject.AddComponent<SphereCollider>();
                    (triggerCollider as SphereCollider).radius = GetComponent<Light>().range;
                    triggerCollider.isTrigger = true;
                    break;
                }
        }
        if(!lightEnabled)
        {
            GetComponent<NavMeshObstacle>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(lightEnabled)
        {
            if (other.tag == affectedEntitiesTag)
            {
                other.GetComponent<Agent>().StunAgent(afflictionDurationSeconds);
            }
        }
    }
}
