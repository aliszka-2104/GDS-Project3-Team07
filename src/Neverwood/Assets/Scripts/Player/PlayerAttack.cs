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
        if (!CanShoot()) return;
        var direction = target - transform.position;
        SendMessage("OnShoot",new Vector2(direction.x,direction.z));
        nextFireTime = Time.time + cooldownTime;
        inventory.RemoveItem(0);

        var projectileSpawn = transform.position + Vector3.up;
        //var obj = Instantiate(projectile, target, Quaternion.identity);
        var obj = Instantiate(projectile, projectileSpawn, Quaternion.identity);
        var projectileComponent = obj.GetComponent<Projectile>();
        if (projectileComponent)
        {
            projectileComponent.GoToTarget(target);
        }
    }

    public bool CanShoot()
    {
        if (!CharacterSwitcher.instance.girl.IsCurrentCharacter) return false;
        if (!inventory.TryGetItem(0)) return false;
        if (Time.time < nextFireTime) return false;

        return true;
    }
}
