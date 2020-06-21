using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    #region Agent parameters

    public StateType startingState;

    #endregion
    #region Private value holders

    Dictionary<StateType, IState> stateDict = new Dictionary<StateType, IState>();
    public StateType currentState { get; set; }
    bool agentTerminating = false;

    #endregion
    #region Unity callbacks

    private void Awake()
    {
        IState[] states = GetComponents<IState>();
        foreach (IState state in states)
        {
            stateDict.Add(state.stateType, state);
        }
        SetState(startingState);
    }
    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    #endregion
    #region State machine

    IEnumerator GameLoop()
    {
        while (!agentTerminating)
        {
            yield return StartCoroutine(stateDict[currentState].StateProcess());
        }
        yield return null;
    }

    public void ChangeState(StateType newState)
    {
        object[] data = stateDict[currentState].Exit();
        stateDict[currentState = newState].Entry(data);
    }
    void SetState(StateType newState)
    {
        stateDict[currentState = newState].Entry();
    }

    public void Stun()
    {
        SetState(StateType.Stunned);
    }

    #endregion
    #region Debug

    GUIStyle debugLabel = new GUIStyle();

    private void OnDrawGizmos()
    {
        debugLabel.fontSize = 32;
        debugLabel.normal.textColor = Color.white;
        if(stateDict.ContainsKey(currentState))
        {
            Handles.Label(this.transform.position + Vector3.up, stateDict[currentState].stateName, debugLabel);
        }
    }

    #endregion
}
