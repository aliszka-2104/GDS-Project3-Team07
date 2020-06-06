using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyInteraction : MonoBehaviour
{
    private Vector2 lastDirection = Vector2.zero;

    public void Raycast(Vector3 direction)
    {
        if (direction.magnitude > 0.1f) lastDirection = direction;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastDirection, 2.0f, LayerMask.GetMask("Enemy"));

        if (hit.collider!=null)
        {
            var enemy = hit.collider.gameObject;

            var script = enemy.GetComponent<EnemyInteraction>();
            script.GetAngry();
        }
    }
}
