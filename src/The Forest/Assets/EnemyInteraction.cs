using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteraction : MonoBehaviour
{
    public GameObject emote;

    private bool isAngry=false;

    public void GetAngry()
    {
        isAngry = true;
    }

    private void LateUpdate()
    {
        if(isAngry)
        {
            emote.SetActive(true);
            isAngry = false;
        }
        else
        {
            emote.SetActive(false);
        }
    }
}
