using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    public void OnAttackPlayer(Collider player)
    {
        Debug.Log("Lost");
        GameManager.instance.ResetLevel();
    }
}
