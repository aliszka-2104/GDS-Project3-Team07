using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireTwinkle : MonoBehaviour
{
    public int minTemperature;
    public int maxTemperature;
    private Light light;
    private float lightTemperature;

    private void Awake()
    {
        light = GetComponent<Light>();
        lightTemperature = light.colorTemperature;
    }

    void Update()
    {
        if (!light.enabled) return;

        light.colorTemperature = Random.Range(minTemperature, maxTemperature);
    }


}
