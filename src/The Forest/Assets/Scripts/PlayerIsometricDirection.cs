using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsometricDirection : MonoBehaviour
{
    string[] states = {"NW", "W","SW","S","SE","E","NE","N"};

    Animator animator;

    string state = "";
    int lastState=0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction)
    {
        if(direction.magnitude<0.1f)
        {
            state = "Idle_";
        }
        else
        {
            state = "Run_";
        lastState = GetIndex(direction);
        }

        state += states[lastState];
        animator.Play(state);
    }

    public int GetIndex(Vector2 direction)
    {
        var step = 360f / 8;
        var halfStep = step / 2;

        var angle = Vector2.SignedAngle(Vector2.up, direction);
        angle += halfStep;
        if (angle < 0) angle += 360f;

        var index = angle / step;
        var i = Mathf.FloorToInt(index);
        return i;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
