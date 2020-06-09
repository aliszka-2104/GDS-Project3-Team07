using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostile : State
{
    private const string name = "Hostile";
    public override string Name
    {
        get
        {
            return name;
        }
    }
    public override void Initialize(AgentDesc agentDesc)
    {
        Debug.Log("Hostile State");
        base.Initialize(agentDesc);
        Debug.Log("End Hostile");
    }
    public override void Start()
    {
        Debug.Log("Hostile Start()");
    }
    public override void Update()
    {

    }
    public override void End()
    {
        Debug.Log("Hostile End()");
    }
}
