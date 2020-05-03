using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 5.0f;

    private Rigidbody2D rBody;
    private Animator animator;
    private SpriteRenderer sprite;
    private bool walking = false;
    private int direction = 0;
    private float hor = 0;
    private float ver = 0;
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        KeyDown();
        KeyUp();
        rBody.velocity = new Vector2(hor * playerSpeed, ver * playerSpeed);
    }
    void KeyDown()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = 1;
            animator.SetInteger("direction", direction);
            animator.SetTrigger("directionChange");
            if (!walking) { animator.SetTrigger("walkingChange"); walking = true; }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            direction = 0;
            animator.SetInteger("direction", direction);
            animator.SetTrigger("directionChange");
            if (!walking) { animator.SetTrigger("walkingChange"); walking = true; }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = 2;
            if (!walking) { animator.SetTrigger("walkingChange"); walking = true; }
            animator.SetInteger("direction", direction);
            animator.SetTrigger("directionChange");
            sprite.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = 2;
            if (!walking) { animator.SetTrigger("walkingChange"); walking = true; }
            animator.SetInteger("direction", direction);
            animator.SetTrigger("directionChange");
            sprite.flipX = false;
        }
    }
    void KeyUp()
    {
        if (Input.GetKeyUp(KeyCode.W) && direction == 1)
        {
            if (walking) { animator.SetTrigger("walkingChange"); walking = false; }
        }
        if (Input.GetKeyUp(KeyCode.S) && direction == 0)
        {
            if (walking) { animator.SetTrigger("walkingChange"); walking = false; }
        }
        if (Input.GetKeyUp(KeyCode.A) && direction == 2 && sprite.flipX)
        {
            if (walking) { animator.SetTrigger("walkingChange"); walking = false; }
        }
        if (Input.GetKeyUp(KeyCode.D) && direction == 2 && !sprite.flipX)
        {
            if (walking) { animator.SetTrigger("walkingChange"); walking = false; }
        }
    }
}
