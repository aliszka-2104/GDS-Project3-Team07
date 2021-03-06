﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Vision))]
[RequireComponent(typeof(Hearing))]
public class Alert : MonoBehaviour, IState
{
    #region State parameters

    [Header("Parameters")]
    public float movementSpeed = 1f;
    public float spotAngle = 50f;
    public float visionRange = 6f;
    public float peripheralVisionRange = 1f;
    public float hearingRange = 9f;
    public int executesPerSecond = 14;
    public float killRange = 1f;
    public float noNewTargetTimeFallOff = 2f;
    public float minTimeInAlertState = 2f;
    public float minAudioGap = 2f;
    public float maxAudioGap = 10f;
    public AudioClip[] soundEffects;
    [Header("State Links")]
    public StateType onTargetSensedStateChange = StateType.Hostile;
    public StateType onTimeFallOffStateChange = StateType.Idle;

    #endregion
    #region Private value holders

    WaitForSeconds waitFor;
    Collider target;
    float noNewTargetTimer;
    float minTimeInAlertTimer;
    private float untilNextSound;

    #endregion
    #region Unity callbacks
    #endregion
    #region Interface implementation

    public string stateName { get; } = "Alert";
    public StateType stateType { get; } = StateType.Alert;
    public void Entry(object[] data = null)                     //Data[1] holds a bool that tells if Alert was changed from Hostile (there's no min time if true)
    {
        if (GetComponent<NavMeshAgent>().enabled) GetComponent<NavMeshAgent>().speed = movementSpeed;
        GetComponent<Vision>().spotAngle = spotAngle;
        GetComponent<Vision>().range = visionRange;
        GetComponent<Vision>().peripheralVisionRange = peripheralVisionRange;
        GetComponent<Hearing>().range = hearingRange;

        target = data[0] as Collider;
        minTimeInAlertTimer = minTimeInAlertState;
        if (data.Length > 1)
        {
            if ((bool)data[1])
            {
                minTimeInAlertTimer = 0f;
            }
        }

        untilNextSound = 0.1f;

        SetDestinationToTarget();
        StartCoroutine(NoNewTargetFallOff(noNewTargetTimeFallOff));
    }
    public object[] Exit()
    {
        return new object[1] { target };
    }
    public IEnumerator StateProcess()
    {
        if (target != null)
        {
            noNewTargetTimer = noNewTargetTimeFallOff;
            SetDestinationToTarget();
        }
        if (GetComponent<Vision>().IsColliderInSight(target))
        {
            KillRangeCheck();
            if (minTimeInAlertTimer < 0)
            {
                GetComponent<Agent>().ChangeState(onTargetSensedStateChange);
            }
        }
        else
        {
            target = null;
            target = TargetSense();
        }
        if (soundEffects.Length > 0)
        {
            if (untilNextSound <= 0f)
            {
                int soundEffectIndex = UnityEngine.Random.Range(0, soundEffects.Length);
                GetComponent<AudioSource>().clip = soundEffects[soundEffectIndex];
                if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();

                untilNextSound = Random.Range(minAudioGap, maxAudioGap);
            }
            untilNextSound -= Time.deltaTime;
        }
        minTimeInAlertTimer -= 1f / executesPerSecond;
        yield return waitFor = new WaitForSeconds(1f / executesPerSecond);
    }

    #endregion
    #region Methods

    IEnumerator NoNewTargetFallOff(float time)
    {
        noNewTargetTimer = noNewTargetTimeFallOff;
        while (noNewTargetTimer > 0)
        {
            noNewTargetTimer -= Time.deltaTime;
            if (GetComponent<Agent>().currentState != this.stateType)
            {
                yield break;
            }
            yield return null;
        }
        GetComponent<Agent>().ChangeState(onTimeFallOffStateChange);
    }
    void SetDestinationToTarget()
    {
        if (GetComponent<NavMeshAgent>().enabled) GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }
    Collider TargetSense()
    {
        Collider sensedTarget = null;
        Collider[] visualPossibleTargets = GetComponent<Vision>().UseSense();
        foreach (Collider coll in visualPossibleTargets)
        {
            if (coll.tag == "Player")
            {
                sensedTarget = coll;
                break;
            }
        }
        if (sensedTarget == null)
        {
            Collider[] auditoryPossibleTargets = GetComponent<Hearing>().UseSense();
            if (auditoryPossibleTargets.Length != 0)
            {
                sensedTarget = auditoryPossibleTargets[0];
            }
        }
        return sensedTarget;
    }
    void KillRangeCheck()
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= killRange)
        {
            if (target.tag == "Player")
            {
                //Application.Quit();
            }
        }
    }

    #endregion
}
