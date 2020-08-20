using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMap : MonoBehaviour
{
    Lantern lantern;
    private void Start()
    {
        lantern = GetComponentInChildren<Lantern>();
    }

    public void OnLight()
    {
        lantern.LanternToggle();
    }
    public void OnInteract()
    {
        var colliders = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Interactive"));
        if (colliders.Length > 0)
        {
            var first = colliders[0];
            if (first.GetComponent<Interactive>())
            {
                first.GetComponent<Interactive>().Interact();
            }
        }
    }
    public void OnAttack()
    {
    }
}
