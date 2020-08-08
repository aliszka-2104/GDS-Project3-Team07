using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Interactive
{
    public GameObject campfireLIght;

    public override void Interact()
    {
        campfireLIght.SetActive(true);
    }

    public void ToggleLight()
    {
        campfireLIght.SetActive(!campfireLIght.activeSelf);
    }
}
