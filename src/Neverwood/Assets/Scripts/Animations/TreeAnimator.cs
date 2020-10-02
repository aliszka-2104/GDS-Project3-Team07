using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimator : MonoBehaviour
{
    public GameObject sprite;

    private Animator animator;
    private Agent agent;

    void Awake()
    {
        agent = GetComponent<Agent>();
        animator = GetComponentInChildren<Animator>();
    }

    void LateUpdate()
    {
        if (agent.currentState == StateType.Idle)
        {
            animator.SetTrigger("Sleep");
        }
        else if (agent.currentState == StateType.Alert)
        {
            animator.SetTrigger("WakeUp");
            animator.ResetTrigger("Sleep");
        }
        else if (agent.currentState == StateType.Stunned)
        {
            animator.SetTrigger("Hit");
        }
    }

    void OnAttackPlayer(Collider player)
    {
        animator.SetTrigger("Attack");
    }
}
