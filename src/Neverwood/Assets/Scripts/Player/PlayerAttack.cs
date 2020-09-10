﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectile;
    public float cooldownTime;

    private Player player;
    private Inventory inventory;
    private GameObject selectedObject;
    private float nextFireTime;

    private void Awake()
    {
        player = GetComponent<Player>();
        inventory = FindObjectOfType<Inventory>();
    }

    public void Shoot(Vector3 target)
    {
        if (Time.time < nextFireTime) return;
        SendMessage("OnShoot");
        nextFireTime = Time.time + cooldownTime;
        inventory.RemoveItem(0);

        var projectileSpawn = transform.position + 2*Vector3.up;
        //var obj = Instantiate(projectile, target, Quaternion.identity);
        var obj = Instantiate(projectile, projectileSpawn, Quaternion.identity);
        if(obj.GetComponent<Projectile>()!=null)
        {
            obj.GetComponent<Projectile>().GoToTarget(target);
        }
    }

    //public Vector3 GetPlayerPlaneMousePos(Vector3 aPlayerPos)
    //{
    //    Plane plane = new Plane(Vector3.up, aPlayerPos);
    //    Ray  ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    float dist;
    //    if (plane.Raycast(ray, out dist))
    //    {
    //        return ray.GetPoint(dist);
    //    }
    //    return Vector3.zero;
    //}

    //public Vector3 GetMousePosition()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray,out hit,1000,LayerMask.GetMask("Ground","NPC")))
    //    {
    //        return hit.point;
    //    }
    //    return Vector3.zero;
    //}
}
