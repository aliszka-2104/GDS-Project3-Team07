using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public GameObject light;

    private Rigidbody rb;
    private CharacterController cc;
    private PlayerDirection playerDirection;
    private Vector3 input = Vector3.zero;
    private Vector3 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        playerDirection = GetComponentInChildren<PlayerDirection>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            light.SetActive(!light.activeSelf);
        }

        moveDirection = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        moveDirection *= speed;
        cc.Move(moveDirection * Time.deltaTime);
        transform.rotation = Quaternion.identity;

        playerDirection.SetDirection(moveDirection);
    }
}
