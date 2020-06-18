using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Networking;

public class Agent : MonoBehaviour
{
    public Light peripheralLight;
    public Light fieldOfVisionLight;

    public string startingState;
    public bool debug;

    public Light[] Lights
    {
        get
        {
            return visionLights;
        }
    }

    Light[] visionLights;
    State[] states;
    int currentState;

    void Awake()
    {
        visionLights = new Light[2]
        {
            peripheralLight,
            fieldOfVisionLight
        };
        states = GetComponents<State>();

        for(int i = 0; i < states.Length; i++)
        {
            if(states[i].Name == startingState)
            {
                currentState = i;
                break;
            }
        }
    }
    void Start()
    {
        states[currentState].Entry();
    }

    public void ChangeState(string name, params object[] data)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].Name == name)
            {
                states[currentState].Exit();
                currentState = i;
                states[currentState].Entry(data);
                break;
            }
            else if (i == states.Length - 1)
            {
                throw new System.ArgumentException("No state with name: " + name + " found");
            }
        }
    }
    public void StunAgent(float time)
    {
        foreach(State state in states)
        {
            if(state.IsStunState)
            {
                if(state.Name != states[currentState].Name)
                {
                    ChangeState(state.Name);
                    break;
                }
            }
        }
        states[currentState].Stun(time);
    }
}
