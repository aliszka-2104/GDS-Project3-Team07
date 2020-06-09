using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.Networking;

public struct AgentDesc
{
    public string name;
    public GameObject gameObject;
    public float movementSpeed;
}
public class Agent : MonoBehaviour
{
    public float movementSpeed = 2f;
    public Transform start;
    public Transform end;

    public int startingState;
    State[] states;
    int currentState;

    void Start()
    {
        GetComponent<PathUnit>().speed = movementSpeed;

        states = new State[3];
        states[0] = new Idle();
        states[1] = new Alert();
        states[2] = new Hostile();

        AgentDesc agentDesc = new AgentDesc();
        agentDesc.name = "Agent";
        agentDesc.gameObject = this.gameObject;
        agentDesc.movementSpeed = movementSpeed;

        foreach(State state in states)
        {
            Debug.Log(state.Name);
            state.Initialize(agentDesc);
        }
        currentState = startingState;
        states[currentState].Start();
    }
    void Update()
    {
        states[currentState].Update();
    }
    public void ChangeState(string name)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].Name == name)
            {
                states[currentState].End();
                currentState = i;
                states[currentState].Start();
                break;
            }
            else if (i == states.Length-1)
            {
                throw new System.ArgumentException("No state with name: " + name + " found");
            }
        }
    }
    public void ChangeState(int stateNr)
    {
        if(stateNr < 0)
        {
            throw new System.ArgumentException("State number is negative.");
        }
        if(stateNr >= states.Length)
        {
            throw new System.ArgumentException("State number exceeds maximum.");
        }
        states[currentState].End();
        currentState = stateNr;
        states[currentState].Start();
    }
}
