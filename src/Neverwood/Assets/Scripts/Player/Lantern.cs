using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lantern : MonoBehaviour
{
    public float lanternLength = 10f;

    public float lanternLeft;

    Light lanternLight;
    Player player;
    NavMeshObstacle obstacle;
    Inventory inventory;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        lanternLeft = lanternLength;
        lanternLight = GetComponent<Light>();
        player = gameObject.GetComponentInParent<Player>();
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
    }
    private void Update()
    {
        if(lanternLight.enabled)
        {
            lanternLeft -= Time.deltaTime;
            if(lanternLeft <= 0f)
            {
                lanternLight.enabled = false;
                obstacle.enabled = false;
            }
        }
    }
    public void UseFuel()
    {
        bool fuelInInven = inventory.TryGetItem(2);
        if(fuelInInven)
        {
            inventory.RemoveItem(2);
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
            obstacle.enabled = !obstacle.enabled;
        }
        else
        {
            UseFuel();
        }
    }
}
