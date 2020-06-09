using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class State
{
    public abstract string Name { get; }
    GameObject gameObject;
    float movementSpeed;

    public GameObject Agent
    {
        get
        {
            return gameObject;
        }
    }
    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
    }

    public virtual void Initialize(AgentDesc agentDesc)
    {
        gameObject = agentDesc.gameObject;
        movementSpeed = agentDesc.movementSpeed;
    }
    public abstract void Start();
    public abstract void Update();
    public abstract void End();
}
