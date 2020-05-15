using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public Vector3 offset;
    public float smoothingSpeed = 5f;
    void Update()
    {
        if (Mathf.Abs(player.position.x - transform.position.x) > 1.5f || Mathf.Abs(player.position.y - transform.position.y) > 1.5f)
        {
            transform.position = Vector3.Lerp(transform.position, player.position + offset, smoothingSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, player.position + offset, smoothingSpeed / 2 * Time.deltaTime);
        }
    }
}
