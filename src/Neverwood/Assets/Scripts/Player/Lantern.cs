using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    public float lanternLength = 10f;

    float lanternLeft;

    Light lanternLight;
    Player player;

    private void Awake()
    {
        lanternLeft = lanternLength;
        lanternLight = GetComponent<Light>();
        player = gameObject.GetComponentInParent<Player>();
    }
    private void Update()
    {
        if(lanternLight.enabled)
        {
            lanternLeft -= Time.deltaTime;
            if(lanternLeft <= 0f)
            {
                lanternLight.enabled = false;
            }
        }
    }
    public void UseFuel()
    {
        bool fuelInInven = player.GetComponent<Inventory>().TryGetItem(2);
        if(fuelInInven)
        {
            player.GetComponent<Inventory>().RemoveItem(2);
            lanternLeft = lanternLength;
            Debug.Log("Fuel loaded");
        }
        else
        {
            Debug.Log("No lantern fuel left");
        }
    }
    public void LanternToggle()
    {
        if(lanternLeft > 0f)
        {
            lanternLight.enabled = !lanternLight.enabled;
        }
        else
        {
            UseFuel();
        }
    }
}
