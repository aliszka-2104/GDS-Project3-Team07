using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
    public GameObject sprite;
    public float flipFactor = 1;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Agent agent;

    private float prevVelX = 0;
    private Quaternion rotation;

    void Awake()
    {
        agent = GetComponent<Agent>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rotation = sprite.transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //sprite.transform.rotation = rotation;

        //if(navMeshAgent.velocity.x>0.5f || navMeshAgent.velocity.x<-0.5f)
        //{
        //    animator.SetBool("Horizontal", true);
        //}
        //else
        //{
        //    animator.SetBool("Horizontal", false);
        //}

        if (agent.currentState == StateType.Stunned)
        {
            Debug.Log("Hit");
            //animator.SetBool("Walking", false);
            //animator.SetTrigger("Hit");
            animator.SetFloat("State", 1);
        }
        else if (navMeshAgent.velocity.magnitude > 0 && navMeshAgent.remainingDistance > 0.1f)
        {
            animator.SetFloat("State", 2);

            //animator.SetBool("Walking", true);
            if (Mathf.Abs(navMeshAgent.velocity.x) > Mathf.Abs(navMeshAgent.velocity.z))
            {
                animator.SetFloat("X", navMeshAgent.velocity.x * flipFactor);
                animator.SetFloat("Z", 0);
            }
            else
            {
                animator.SetFloat("X", 0);
                animator.SetFloat("Z", navMeshAgent.velocity.z);
            }
        }
        else
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
}
