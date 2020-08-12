using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Transforms to act as start and end markers for the journey.
    public Vector3 start;
    public Vector3 target;

    public GameObject audioCue;
    public GameObject dustCloud;

    // Movement speed in units per second.
    public float speed = 1.0F;

    private bool moving = false;

    void Start()
    {
        start = transform.position;
    }

    // Move to the target end position.
    void Update()
    {
        if (!moving) return;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    internal void GoToTarget(Vector3 target)
    {
        //this.target = new Vector3(target.x,0,target.z);
        this.target = target;
        moving = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Instantiate(audioCue, transform.position, Quaternion.identity);
            Instantiate(dustCloud, transform.position, Quaternion.identity);
        }
        Debug.Log("Hit with "+collision.gameObject.name);
        Destroy(gameObject);
    }
}
