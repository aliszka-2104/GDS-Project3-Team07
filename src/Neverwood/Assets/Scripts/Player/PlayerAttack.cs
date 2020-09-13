using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectile;
    public float cooldownTime;
    //public string[] layerNames = {"Ground", "NPC"};

    private Inventory inventory;
    private float nextFireTime;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void Shoot(Vector3 target)
    {
        if (!inventory.TryGetItem(0)) return;
        if (Time.time < nextFireTime) return;
        SendMessage("OnShoot");
        nextFireTime = Time.time + cooldownTime;
        inventory.RemoveItem(0);

        var projectileSpawn = transform.position + 2*Vector3.up;
        //var obj = Instantiate(projectile, target, Quaternion.identity);
        var obj = Instantiate(projectile, projectileSpawn, Quaternion.identity);
        var projectileComponent = obj.GetComponent<Projectile>();
        if (projectileComponent)
        {
            projectileComponent.GoToTarget(target);
        }
    }
}
