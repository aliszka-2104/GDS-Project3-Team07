using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : State
{
    private const string name = "Alert";
    public override string Name
    {
        get
        {
            return name;
        }
    }
    public override void Initialize(AgentDesc agentDesc)
    {
        Debug.Log("Alert State");
        base.Initialize(agentDesc);
        Debug.Log("End Alert");
    }
    public override void Start()
    {
        Debug.Log("Alert Start()");
    }
    public override void Update()
    {

    }
    public override void End()
    {
        Debug.Log("Alert End()");
    }
}
