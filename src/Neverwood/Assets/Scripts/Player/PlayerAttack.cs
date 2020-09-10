using System.Collections;
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
}
