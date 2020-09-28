using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLantern : Interactive
{
    public event Action LanternHit;
    public event Action LanternPickedUp;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            if (other) Destroy(other.gameObject);
            GetComponent<Animator>().SetTrigger("FallDown");
            gameObject.layer = LayerMask.NameToLayer("Interactive");
            LanternHit?.Invoke();
        }
    }

    public override void Interact()
    {
        if (!CharacterSwitcher.instance.boy.IsCurrentCharacter) return;
        LanternPickedUp?.Invoke();
    }
}
