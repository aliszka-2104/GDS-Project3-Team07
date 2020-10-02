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
    private Collider hearingCollider;
    private Vector3 input = Vector3.zero;
    private Vector3 moveDirection;
    private Collider[] myColliders;
    private Vector2 movementVector = Vector2.zero;
    private float audioStartTime = 1.5f;
    private float audioStopTime = 0.5f;
    private float timeToAudioStart;
    private float timeToAudioStop;

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

    private void OnEnable()
    {
        hearingCollider = transform.Find("Colliders").Find("Hearing").GetComponent<Collider>();
        timeToAudioStart = audioStartTime;
        timeToAudioStop = 0f;
    }

    void Update()
    {
        if (movingTo)
        {
            StartCoroutine(Lerp());
        }
        else
        {
            Move();
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
    public void ChangeDirection(Vector2 inputVector)
    {
        movementVector = inputVector;
    }
    private void Move()
    {
        if (Stunned) return;

        if (movementVector == Vector2.zero)
        {
            timeToAudioStart = audioStartTime;
            hearingCollider.enabled = false;
            cc.Move(Vector2.zero);
            return;
        }

        if (timeToAudioStart > 0f) timeToAudioStart -= Time.deltaTime;
        else if (!hearingCollider.enabled)
        {
            timeToAudioStop = audioStopTime;
            hearingCollider.enabled = true;
        }
        else if (timeToAudioStop > 0f) timeToAudioStop -= Time.deltaTime;
        else
        {
            timeToAudioStart = audioStartTime;
            hearingCollider.enabled = false;
        }


        moveDirection = new Vector3(movementVector.x, 0, movementVector.y * 1.5f);
        moveDirection *= speed;
        cc.Move(moveDirection * Time.deltaTime);
        transform.rotation = Quaternion.identity;

        playerDirection.SetDirection(moveDirection);
        if (transform.position.y != 0) transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
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
