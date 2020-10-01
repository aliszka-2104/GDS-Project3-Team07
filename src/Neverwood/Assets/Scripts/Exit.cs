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
            if (sceneName == "End")
            {
                Debug.Log("Won");
                this.gameObject.AddComponent<Ending>();
            }
            else if (sceneName == "Level2" && Inventory.instance.TryGetItem(3))
            {
                Debug.Log("Won");
                GameManager.instance.LoadLevel(sceneName);
            }
            exited = true;
        }
    }
}
