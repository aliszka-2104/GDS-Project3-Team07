using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public string sceneName = "Level1";
    public bool Open { get; set; } = false;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Open)
        {
            Debug.Log("Won");
            GameManager.instance.LoadLevel(sceneName);
        }
    }
}
