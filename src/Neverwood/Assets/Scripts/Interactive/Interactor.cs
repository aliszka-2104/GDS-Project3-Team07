using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Vision))]
public class Interactor : MonoBehaviour
{
    Vision vision;
    public KeyCode interactKey = KeyCode.E;
    private void Awake()
    {
        vision = GetComponent<Vision>();
        vision.range = 1;
        vision.peripheralVisionRange = 0;
        vision.spotAngle = 120;
        vision.senseMask = ~0;
        vision.senseBlockingMask = ~vision.senseMask;
    }
    private void Update()
    {
        if(Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }
    void Interact()
    {
        Collider[] interactible = vision.UseSense().Where(elem => elem.GetComponent<Interactive>() != null).ToArray();
        if (interactible.Length > 0)
        {
            interactible[0].GetComponent<Interactive>().Interact();
        }
    }
}
