using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class ProximityTrigger : MonoBehaviour
{
    public float triggerRadius;
    public UnityEvent triggered;

    SphereCollider trigger;
    private void Awake()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = triggerRadius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Triggered(other);
        }
    }
    public virtual void Triggered(Collider other)
    {
        triggered.Invoke();
    }
}
