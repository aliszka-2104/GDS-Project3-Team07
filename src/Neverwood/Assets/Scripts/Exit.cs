using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public string sceneName = "Level1";

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Won");
            GetComponentInChildren<Animator>().SetTrigger("Open");
            GameManager.instance.LoadLevel(sceneName);
        }
    }
}
