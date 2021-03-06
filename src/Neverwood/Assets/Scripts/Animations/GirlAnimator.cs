﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlAnimator : MonoBehaviour
{
    public GameObject sprite;
    public float flipFactor = 1;

    private Animator animator;
    private CharacterController cc;

    private Quaternion rotation;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        rotation = sprite.transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cc.velocity.magnitude > 0)
        {
            animator.SetFloat("State", 0);

            animator.SetBool("Walking", true);

            float velX = Mathf.Abs(cc.velocity.x);
            float velZ = Mathf.Abs(cc.velocity.z);
            if ( velZ> velX ||velX-velZ<0.1f)
            {
                animator.SetFloat("X", 0);
                animator.SetFloat("Z", cc.velocity.z);
            }
            else
            {
                animator.SetFloat("X", cc.velocity.x * flipFactor);
                animator.SetFloat("Z", 0);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
            animator.SetFloat("State", -1);

        }

        //sprite.transform.rotation = rotation;

        //if(navMeshAgent.velocity.x>0.5f || navMeshAgent.velocity.x<-0.5f)
        //{
        //    animator.SetBool("Horizontal", true);
        //}
        //else
        //{
        //    animator.SetBool("Horizontal", false);
        //}

        //if (agent.currentState == StateType.Stunned)
        //{
        //    Debug.Log("Hit");
        //    //animator.SetBool("Walking", false);
        //    //animator.SetTrigger("Hit");
        //    animator.SetFloat("State", 1);
        //}
        //else if (navMeshAgent.velocity.magnitude > 0 && navMeshAgent.remainingDistance > 0.1f)
        //{
        //    animator.SetFloat("State", 2);

        //    //animator.SetBool("Walking", true);
        //    if (Mathf.Abs(navMeshAgent.velocity.x) > Mathf.Abs(navMeshAgent.velocity.z))
        //    {
        //        animator.SetFloat("X", navMeshAgent.velocity.x * flipFactor);
        //        animator.SetFloat("Z", 0);
        //    }
        //    else
        //    {
        //        animator.SetFloat("X", 0);
        //        animator.SetFloat("Z", navMeshAgent.velocity.z);
        //    }
        //}
        //else
        {
            //animator.SetFloat("State", 1);

            //animator.SetBool("Walking", false);
        }

        //Debug.Log("Vel X " + navMeshAgent.velocity.x);
        //Debug.Log("Vel Z " + navMeshAgent.velocity.z);
        //Debug.Log("Dist " + navMeshAgent.remainingDistance);

        sprite.transform.rotation = rotation;
    }

    void OnAttackPlayer(Collider player)
    {
        if (animator.GetFloat("State") != 3) animator.SetFloat("State", 3);
        //animator.SetBool("Walking", false);
        //animator.SetTrigger("Attack");
    }

    void OnCrossObstacle()
    {
        var x = Mathf.Abs(cc.velocity.x) > 0 ? Mathf.Sign(cc.velocity.x) : 1;
        animator.SetTrigger("Crawl");
        animator.SetFloat("X", x);
    }
    void OnShoot(Vector2 direction)
    {
        if (direction.y > direction.x || direction.x - direction.y < 0.1f)
        {
            animator.SetFloat("X", 0);
            animator.SetFloat("Z", direction.y);
        }
        else
        {
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Z", 0);
        }
        animator.SetTrigger("Attack");
    }
}
