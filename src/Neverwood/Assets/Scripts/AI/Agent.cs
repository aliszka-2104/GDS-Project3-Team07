using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public Light peripheralLight;
    public Light fieldOfVisionLight;
    public StateDesc[] stateDescs;
    public int startingState;
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

        CreateStatesFromSet();
        currentState = startingState;
        states[currentState].Start();
    }
    void Update()
    {
        states[currentState].Update();
    }
    private void OnDrawGizmos()
    {
        if (visionLights != null)
        {
            Gizmos.color = Lights[0].color;
            Gizmos.DrawRay(transform.position, transform.forward * Lights[1].range);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, Lights[1].spotAngle/2, 0) * transform.forward * Lights[1].range);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -Lights[1].spotAngle/2, 0) * transform.forward * Lights[1].range);
            Gizmos.DrawWireSphere(transform.position, Lights[0].range);
        }
        if (states != null && debug)
        {
            states[currentState].DebugGizmos();
        }
    }
    private void OnApplicationQuit()
    {
        visionLights = null;
    }


    public void ChangeState(string name, float delay = 0f, params object[] data)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].Name == name)
            {
                StartCoroutine(OnStateChange(i, delay, data));
                break;
            }
            else if (i == states.Length - 1)
            {
                throw new System.ArgumentException("No state with name: " + name + " found");
            }
        }
    }
    void CreateStatesFromSet()
    {
        states = new State[stateDescs.Length];
        for (int stateDescIter = 0; stateDescIter < stateDescs.Length; stateDescIter++)
        {
            StateDesc stateDesc = stateDescs[stateDescIter];
            states[stateDescIter] = Activator.CreateInstance(Type.GetType(stateDesc.stateName), this.gameObject, stateDesc) as State;
        }
    }
    IEnumerator OnStateChange(int newState, float delay = 0f, params object[] data)
    {
        yield return new WaitForSeconds(delay);
        states[currentState].End();
        currentState = newState;
        states[currentState].Start(data);
    }
}
