using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            this.gameObject.layer = LayerMask.NameToLayer("Hearing");
            this.gameObject.AddComponent<AuditoryCue>().length = 1.5f;
            Destroy(this);
        }
    }
}
