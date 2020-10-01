using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public string sceneName = "Level1";
    public bool Open { get; set; } = true;

    private int charactersToGo = 2;

    bool exited = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            charactersToGo--;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            charactersToGo++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Open && charactersToGo == 0 && !exited)
        {
            Debug.Log("Won");
            if (sceneName == "End")
            {
                this.gameObject.AddComponent<Ending>();
            }
            else
            {
                GameManager.instance.LoadLevel(sceneName);
            }
            exited = true;
        }
    }
}
