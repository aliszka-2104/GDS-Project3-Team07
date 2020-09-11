using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stunned : MonoBehaviour, IState                //Dummy state for stun to be changed
{
    #region State parameters

    #endregion
    #region Private value holders 

    WaitForSeconds waitFor;
    float stunTime = 2f;

    #endregion
    #region Unity callbacks
    #endregion
    #region Interface implementation

    public string stateName { get; } = "Stunned";

    public StateType stateType { get; } = StateType.Stunned;

    public void Entry(object[] data = null)
    {
        SendMessage("OnStunned", null, SendMessageOptions.DontRequireReceiver);
        stunTime = (float)data[0];
        if (GetComponent<NavMeshAgent>().enabled) GetComponent<NavMeshAgent>().ResetPath();
    }

    public object[] Exit()
    {
        return null;
    }

    public IEnumerator StateProcess()
    {
        yield return waitFor = new WaitForSeconds(stunTime);
        if (GetComponent<Agent>().currentState == this.stateType)
        {
            GetComponent<Agent>().ChangeState(StateType.Idle);
        }
    }

    #endregion
    #region Methods

    #endregion
}
