using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : Interactive
{
    public override void Interact()
    {
        CharacterSwitcher.instance.boy.GetComponent<Inventory>().AddItem(2);
        Destroy(this.gameObject);
    }
}
