using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactive
{
    public override void Interact()
    {
        Inventory.instance.AddItem(3);
        Destroy(this.gameObject);
    }
}
