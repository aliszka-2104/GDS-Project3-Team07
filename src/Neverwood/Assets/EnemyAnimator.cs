using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
    public GameObject sprite;
    public NavMeshAgent agent;
    public Animator animator;

    private float prevVelX = 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        sprite.transform.rotation = Quaternion.identity;
        //animator.SetFloat("X", agent.velocity.x);
        animator.SetFloat("Z", agent.velocity.z);

        Debug.Log("Vel X "+agent.velocity.x);
        Debug.Log("Vel Z "+agent.velocity.z);

        if(agent.velocity.x*prevVelX<0)
        {
            Flip();
        }
        prevVelX = agent.velocity.x;
    }

    void Flip()
    {
        sprite.transform.Rotate(Vector3.up * 180);
    }
}
