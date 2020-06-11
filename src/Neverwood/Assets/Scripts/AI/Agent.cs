using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public struct StateDesc
{
    [System.Serializable]
    public struct StateParameter
    {
        public string variableName;
        public string variableValue;
    }
    public string stateName;
    public StateParameter[] stateParameters;
}
public class Agent : MonoBehaviour
{
    public StateDesc[] stateDescs;
    public int startingState;
    public bool debug;

    State[] states;
    int currentState;

    void Awake()
    {
        CreateStatesFromSet();
    }
    void Start()
    {
        currentState = startingState;
        states[currentState].Start();
    }
    void CreateStatesFromSet()
    {
        states = new State[stateDescs.Length];
        for(int stateDescIter = 0; stateDescIter < stateDescs.Length; stateDescIter++)
        {
            StateDesc stateDesc = stateDescs[stateDescIter];
            states[stateDescIter] = Activator.CreateInstance(Type.GetType(stateDesc.stateName), this.gameObject, stateDesc) as State;
        }
    }
    void Update()
    {
        states[currentState].Update();
    }
    private void OnDrawGizmos()
    {
        if(states != null && debug)
        {
            states[currentState].DebugGizmos();
        }
    }


    public void ChangeState(string name, params object[] data)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].Name == name)
            {
                states[currentState].End();
                currentState = i;
                states[currentState].Start(data);
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
    }   //To update for params
}
