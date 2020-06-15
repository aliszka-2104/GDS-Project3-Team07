using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class State
{
    public string nextStateName;
    public abstract string Name { get; }
    GameObject gameObject;
    Agent aiAgent;
    string[] linkedStateNames;

    public Agent AiAgent
    {
        get
        {
            return aiAgent;
        }
    }
    public string[] LinkedStateNames
    {
        get; set;
    }

    public State(GameObject agent, StateDesc stateDesc = new StateDesc())
    {
        gameObject = agent;
        aiAgent = agent.GetComponent<Agent>();
    }
    public abstract void Start(params object[] data);
    public abstract void Update();
    public abstract void End();
    public virtual void DebugGizmos() { }

    public string GetParameterFromName(string name, StateDesc.StateParameter[] stateParameters)
    {
        StateDesc.StateParameter parameter = Array.Find<StateDesc.StateParameter>(stateParameters, elem => elem.variableName == name);
        return parameter.variableValue;
    }
}
