using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class State : MonoBehaviour
{
    public abstract string Name { get; }
    public abstract bool IsStunState { get; }
    public Agent AiAgent
    {
        get; set;
    }

    public string[] linkedStateNames;

    public bool Exiting { get; set; } = false; 
    public WaitForEndOfFrame EndOfFrameYield { get; set; }

    private void Awake()
    {
        AiAgent = GetComponent<Agent>();
    }

    public virtual void Entry(params object[] data)
    {
        StartCoroutine(Step());
    }
    public virtual void Exit()
    {
        Exiting = true;
    }
    public abstract IEnumerator Step();
    public virtual void Stun(float time) { }
    public virtual void DebugGizmos() { }
}
