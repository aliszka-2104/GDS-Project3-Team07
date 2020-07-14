using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum OBSTACLE
    {
        ROCK,
        WOOD
    }

    public Transform target;
    public OBSTACLE obstacleType;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerMovement>()) return;

        other.GetComponent<PlayerMovement>().CrossObstacle(target.position, obstacleType);
    }
}
