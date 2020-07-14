using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Pickup : ProximityTrigger
{
    public string itemToAdd;
    public override void Triggered(Collider other)  //It knows collider is the player already from base class
    {
        base.Triggered(other);
        //other.GetComponent<Player>().AddItem(itemToAdd);
    }
}
