using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLantern : Interactive
{
    public event Action LanternHit;
    public event Action LanternPickedUp;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            GetComponent<Animator>().SetTrigger("FallDown");
            gameObject.layer = LayerMask.NameToLayer("Interactive");
            LanternHit?.Invoke();
        }
    }

    public override void Interact()
    {
        LanternPickedUp?.Invoke();
    }
}
