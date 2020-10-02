using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : Interactive
{
    public override void Interact()
    {
        Inventory.instance.AddItem(2);
        Destroy(this.gameObject);

    }
}
