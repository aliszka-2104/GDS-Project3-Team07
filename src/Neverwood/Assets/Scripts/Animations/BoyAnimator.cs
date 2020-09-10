using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyAnimator : MonoBehaviour
{
    public GameObject sprite;
    public float flipFactor = 1;
    public float[] lanternStates={1,0};

    private Animator animator;
    private CharacterController cc;
    private Quaternion rotation;
    private int currentLanternStateIndex=0;

    public void UpdateLanternState()
    {
        animator.SetFloat("Lantern", lanternStates[currentLanternStateIndex]);
    }

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        rotation = sprite.transform.rotation;
        UpdateLanternState();
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
            if (velZ > velX || velX - velZ < 0.1f)
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
            //animator.SetFloat("State", -1);

        }
        
        sprite.transform.rotation = rotation;
    }

    void OnAttackPlayer(Collider player)
    {
        //if (animator.GetFloat("State") != 3) animator.SetFloat("State", 3);
        //animator.SetBool("Walking", false);
        //animator.SetTrigger("Attack");
    }

    void OnCrossObstacle()
    {
        animator.SetTrigger("Jump");
    }

    void OnLanternToggle()
    {
        currentLanternStateIndex++;
        currentLanternStateIndex = currentLanternStateIndex % lanternStates.Length;
        UpdateLanternState();
        //if (animator.GetFloat("Lantern")==0)animator.SetFloat("Lantern", 1);
        //if(animator.GetFloat("Lantern")==1)animator.SetFloat("Lantern", 0);
    }
}
