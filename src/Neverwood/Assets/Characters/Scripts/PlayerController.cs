using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerWalkState WalkState = new PlayerWalkState();

    private PlayerBaseState currentState;

    void Start()
    {
        Transition(IdleState);
    }

    void Update()
    {
        currentState.Update(this);
    }

    public void Transition(PlayerBaseState state)
    {
        currentState = state;
        state.Enter(this);
    }
}
