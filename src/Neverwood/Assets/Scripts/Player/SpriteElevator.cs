using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteElevator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Transform sprite = other.transform.Find("Visuals");
            sprite.position = new Vector3(sprite.position.x, 1, sprite.position.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Transform sprite = other.transform.Find("Visuals");
            sprite.position = new Vector3(sprite.position.x, 0, sprite.position.z);
        }
    }
}
