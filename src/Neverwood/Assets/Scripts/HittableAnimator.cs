using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableAnimator : Hittable
{
    private Animator animator;

    public override void GetHit()
    {
        animator.SetTrigger("Hit");
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

}
