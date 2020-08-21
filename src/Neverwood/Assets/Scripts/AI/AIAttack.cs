using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    public void OnAttackPlayer(Collider player)
    {
        if (player.GetComponent<Player>())
            player.GetComponent<Player>().TakeHit();
        Debug.Log("Lost");
        GameManager.instance.ResetLevel();
    }
}
