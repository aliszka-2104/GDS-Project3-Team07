using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
    public GameObject sprite;

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
        sprite.transform.rotation = rotation;
        animator.SetFloat("X", navMeshAgent.velocity.x);
        animator.SetFloat("Z", navMeshAgent.velocity.z);

        if(navMeshAgent.velocity.x>0.5f || navMeshAgent.velocity.x<-0.5f)
        {
            animator.SetBool("Horizontal", true);
        }
        else
        {
            animator.SetBool("Horizontal", false);
        }

        if (navMeshAgent.velocity.magnitude>0 && navMeshAgent.remainingDistance>0.1f)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        Debug.Log("Vel X " + navMeshAgent.velocity.x);
        Debug.Log("Vel Z " + navMeshAgent.velocity.z);
        Debug.Log("Dist " + navMeshAgent.remainingDistance);

        //if(agent.velocity.x*prevVelX<0)
        //{
        //    Flip();
        //}
        //prevVelX = agent.velocity.x;
        //sprite.transform.rotation = Quaternion.identity;
        sprite.transform.rotation = rotation;
    }

    void Flip()
    {
        sprite.transform.Rotate(Vector3.up * 180);
    }
}
