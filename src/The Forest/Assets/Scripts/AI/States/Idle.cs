using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private float pathUpdateTime = 0.05f;
    private float timeLeft;

    private const string name = "Idle";
    public override string Name
    {
        get
        {
            return name;
        }
    }
    public override void Initialize(AgentDesc agentDesc)
    {
        Debug.Log("Idle State");
        base.Initialize(agentDesc);
        Debug.Log("End Idle");
    }
    public override void Start()
    {
        Debug.Log("Idle Start()");
        Agent.GetComponent<PathUnit>().GoToTarget();
        timeLeft = pathUpdateTime;
    }
    public override void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            Agent.GetComponent<PathUnit>().GoToTarget();
            timeLeft = pathUpdateTime;
        }
        if(Vector3.Distance(Agent.transform.position, Agent.GetComponent<Agent>().start.position) < 0.5f)
        {
            Agent.GetComponent<PathUnit>().target = Agent.GetComponent<Agent>().end;
            Agent.GetComponent<PathUnit>().GoToTarget();
        }
        if(Vector3.Distance(Agent.transform.position, Agent.GetComponent<Agent>().end.position) < 0.5f)
        {
            Agent.GetComponent<PathUnit>().target = Agent.GetComponent<Agent>().start;
            Agent.GetComponent<PathUnit>().GoToTarget();
        }
    }
    public override void End()
    {
        Debug.Log("Idle End()");
        Agent.GetComponent<PathUnit>().Stop();
    }
}
