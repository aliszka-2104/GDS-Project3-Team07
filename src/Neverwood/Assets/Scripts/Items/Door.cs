using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactive
{
    public int keysNeeded = 1;
    int keysLeft;
    private void Awake()
    {
        keysLeft = keysNeeded;
    }
    public override void Interact()
    {
        if(CharacterSwitcher.instance.boy.GetComponent<Inventory>().TryGetItem(3))
        {
            CharacterSwitcher.instance.boy.GetComponent<Inventory>().RemoveItem(3);
            keysLeft--;
        }
        if (keysLeft == 0)
        {
            GetComponent<Collider>().isTrigger = true;
            GetComponentInChildren<Animator>().SetTrigger("Open");
            GetComponent<Exit>().Open = true;
        }
    }
}
