﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Obstacle;

public class PlayerMovement : MonoBehaviour
{
    public List<OBSTACLE> obstaclesICanCross = new List<OBSTACLE>();
    public float speed = 5f;
    public GameObject light;
    private bool isCurrentCharacter = false;

    private Rigidbody rb;
    private CharacterController cc;
    private Camera camera;
    private PlayerDirection playerDirection;
    private Vector3 input = Vector3.zero;
    private Vector3 moveDirection;

    public bool movingTo = false;
    public Vector3 myTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        SetCurrentCharacter(false);
        playerDirection = GetComponentInChildren<PlayerDirection>();
    }

    void Update()
    {
        if (!isCurrentCharacter)
        {
            cc.Move(Vector3.zero);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            light.SetActive(!light.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var colliders = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Campfire"));

            if (colliders.Length > 0)
            {
                var first = colliders[0];

                if (first.GetComponent<Campfire>())
                {
                    first.GetComponent<Campfire>().ToggleLight();
                }
            }
        }
        if (movingTo)
        {
            Teleport(myTarget);
        }
        else
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection *= speed;
            cc.Move(moveDirection * Time.deltaTime);
            transform.rotation = Quaternion.identity;

            playerDirection.SetDirection(moveDirection);
        }
    }

    public void SetCurrentCharacter(bool isCurrent)
    {
        isCurrentCharacter = isCurrent;
        if (isCurrent)
        {
            camera.gameObject.SetActive(true);
            return;
        }
        camera.gameObject.SetActive(false);
    }

    public void CrossObstacle(Vector3 target, OBSTACLE obstacleType)
    {
        if (!obstaclesICanCross.Contains(obstacleType)) return;
        if (movingTo) return;
        movingTo = true;
        myTarget = new Vector3(target.x, 0, target.z);
        //Teleport(myTarget);
    }

    void Teleport(Vector3 target)
    {
        cc.enabled = false;
        transform.position = target;

        cc.enabled = true;
        movingTo = false;
    }

    void MoveTowardsTarget(Vector3 target)
    {
        var offset = target - transform.position;
        //collider.enabled = false;
        if (offset.magnitude > .1f)
        {
            offset = offset.normalized * speed;
            cc.Move(offset * Time.deltaTime);
        }
        else
        {
            movingTo = false;
            //collider.enabled = true;
        }
    }
}
