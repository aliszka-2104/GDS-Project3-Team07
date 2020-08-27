using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject lantern;

    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (!player.IsCurentCharacter) return;

        if (Input.GetKeyDown(KeyCode.E) &&lantern!=null)
        {
            //SendMessage("OnLight");
            //lantern.SetActive(!lantern.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Q))
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
    }
}
