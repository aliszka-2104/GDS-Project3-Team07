using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Interactive
{
    private Inventory inventory;
    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public override void Interact()
    {
        inventory.AddItem(0);
    }
}
