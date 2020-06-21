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

    #endregion
    #region Unity callbacks
    #endregion
    #region Interface implementation

    public string stateName { get; } = "Stunned";

    public StateType stateType { get; } = StateType.Stunned;

    public void Entry(object[] data = null)
    {
        GetComponent<NavMeshAgent>().ResetPath();
    }

    public object[] Exit()
    {
        return null;
    }

    public IEnumerator StateProcess()
    {
        yield return waitFor = new WaitForSeconds(3f);
        GetComponent<Agent>().ChangeState(StateType.Idle);
    }

    #endregion
    #region Methods

    #endregion
}
