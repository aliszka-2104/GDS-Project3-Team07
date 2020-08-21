using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttack : MonoBehaviour
{
    public float killTime = 5f;

    float timeLeft;
    bool attacking = false;
    Collider playerCollider;
    private void Awake()
    {
        timeLeft = killTime;
    }
    public void OnAttackPlayer(Collider player)
    {
        if(!attacking)
        {
            playerCollider = player;
            player.GetComponent<PlayerMovement>().Stunned = true;
            attacking = true;
            timeLeft = killTime;
            StartCoroutine(killTimer());
        }

    }

    public void OnStunned()
    {
        timeLeft = 0f;
        attacking = false;
        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerMovement>().Stunned = false;
        }
    }
    IEnumerator killTimer()
    {
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        if(attacking)
        {
            Debug.Log("Lost");
            GameManager.instance.ResetLevel();
        }
        attacking = false;
        yield return null;
    }
}
