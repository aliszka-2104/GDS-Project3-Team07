using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public GameObject light;

    public void ToggleLight()
    {
        light.SetActive(!light.activeSelf);
    }
}
