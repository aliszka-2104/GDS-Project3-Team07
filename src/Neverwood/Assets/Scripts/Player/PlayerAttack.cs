using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectile;
    public float cooldownTime;
    public string[] layerNames = {"Ground", "NPC"};

    private Player player;
    private Inventory inventory;
    private GameObject selectedObject;
    private float nextFireTime;

    private void Awake()
    {
        player = GetComponent<Player>();
        inventory = FindObjectOfType<Inventory>();
    }
    
    void Update()
    {
        if (!player.IsCurentCharacter) return;
        if (!Input.GetMouseButtonDown(0)) return;
        if (Time.time<nextFireTime) return;
        if (!inventory.TryGetItem(0)) return;
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hitData;

        //if (Physics.Raycast(ray, out hitData, 1000))
        //{
        //    worldPosition = hitData.point;
        //    Shoot(worldPosition);
        //}

        Shoot(GetMousePosition());
    }

    void Shoot(Vector3 target)
    {
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

    public Vector3 GetPlayerPlaneMousePos(Vector3 aPlayerPos)
    {
        Plane plane = new Plane(Vector3.up, aPlayerPos);
        Ray  ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        if (plane.Raycast(ray, out dist))
        {
            return ray.GetPoint(dist);
        }
        return Vector3.zero;
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,1000,LayerMask.GetMask(layerNames)))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
