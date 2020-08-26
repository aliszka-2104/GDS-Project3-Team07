using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Obstacle;

public class PlayerMovement : MonoBehaviour
{
    public List<OBSTACLE> obstaclesICanCross = new List<OBSTACLE>();
    public bool Stunned { get; set; } = false;
    public float speed = 5f;
    public float lerpDuration = .5f;

    private Player player;
    private Rigidbody rb;
    private CharacterController cc;
    private PlayerDirection playerDirection;
    private Vector3 input = Vector3.zero;
    private Vector3 moveDirection;
    private Collider[] myColliders;

    public bool movingTo = false;
    private Vector3 myTarget;

    void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        playerDirection = GetComponentInChildren<PlayerDirection>();
        myColliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        if (!player.IsCurentCharacter  || Stunned)
        {
            cc.Move(Vector3.zero);
            return;
        }

        if (movingTo)
        {
            StartCoroutine("Lerp");
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


    public void CrossObstacle(Vector3 target, OBSTACLE obstacleType)
    {
        if (!obstaclesICanCross.Contains(obstacleType)) return;
        if (movingTo) return;
        SendMessage("OnCrossObstacle");
        movingTo = true;
        myTarget = new Vector3(target.x, 0, target.z);
    }

    //void Teleport(Vector3 target)
    //{
    //    cc.enabled = false;
    //    transform.position = target;

    //    cc.enabled = true;
    //    movingTo = false;
    //}

    //void MoveTowardsTarget(Vector3 target)
    //{
    //    var offset = target - transform.position;
    //    foreach (var collider in myColliders) collider.enabled = false;
    //    if (offset.magnitude > .1f)
    //    {
    //        offset = offset.normalized * speed;
    //        cc.Move(offset * Time.deltaTime);
    //    }
    //    else
    //    {
    //        movingTo = false;
    //        foreach (var collider in myColliders) collider.enabled = true;
    //    }
    //}

    void OnTakeHit()
    {
        Stunned = true;
    }

    IEnumerator Lerp()
    {
        float timeElapsed = 0;
        var myStartPosition = transform.position;
        //foreach (var collider in myColliders) collider.enabled = false;

        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(myStartPosition, myTarget, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        //foreach (var collider in myColliders) collider.enabled = true;
        transform.position = myTarget;
        movingTo = false;
    }
}
