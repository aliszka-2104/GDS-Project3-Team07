using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttack : MonoBehaviour
{
    public float killTime = 5f;

    float timeLeft;
    bool attacking = false;
    private void Awake()
    {
        timeLeft = killTime;
    }
    public void OnAttackPlayer(Collider player)
    {
        if(!attacking)
        {
            player.GetComponent<PlayerMovement>().stunned = true;
            attacking = true;
            timeLeft = killTime;
            StartCoroutine(killTimer());
        }
    }

    public void OnTreeStunned()
    {
        timeLeft = 0f;
        attacking = false;
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
            Debug.Log("Tree Attack Finished");
        }
        attacking = false;
        yield return null;
    }
}
