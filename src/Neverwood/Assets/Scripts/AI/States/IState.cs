using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Stunned = -1,
    Idle = 0,
    Alert = 1,
    Hostile = 2
}

public interface IState
{
    string stateName { get; }
    StateType stateType { get; }

    void Entry(object[] data = null);
    object[] Exit();
    IEnumerator StateProcess();
}
