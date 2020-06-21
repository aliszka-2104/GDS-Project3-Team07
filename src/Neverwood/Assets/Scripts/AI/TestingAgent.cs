using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Vision))]
public class TestingAgent : MonoBehaviour
{
    public enum StateType
    {
        Idle,
        Alert,
        Hostile
    };
    public StateType state;

    private Collider target;

    private void Update()
    {
        switch (state)
        {
            case StateType.Idle:
                IdleState();
                break;
            case StateType.Alert:
                AlertState();
                break;
            case StateType.Hostile:
                HostileState();
                break;
        }
    }
    private void IdleState()
    {

    }
    private void AlertState()
    {
        if(GetComponent<NavMeshAgent>().isStopped)
        {
            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
        }
    }
    private void HostileState()
    {
        GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
        if(Vector3.Distance(target.transform.position, transform.position) > 5f)
        {
            state = StateType.Idle;
        }
    }
    public void TargetFound(Collider coll)
    {
        if(coll.tag == "Player")
        {
            state = StateType.Hostile;
            target = coll;
        }
    }
    public void TargetHeard(Collider coll)
    {
        if(coll.tag == "Player")
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            state = StateType.Alert;
            target = coll;
        }
    }
}
