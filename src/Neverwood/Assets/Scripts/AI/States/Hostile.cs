using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Vision))]
public class Hostile : MonoBehaviour, IState
{
    #region State parameters

    [Header("Parameters")]
    public float movementSpeed = 2.5f;
    public float spotAngle = 60f;
    public float visionRange = 6f;
    public float peripheralVisionRange = 2f;
    public float hearingRange = 4f;
    public int executesPerSecond = 20;
    public float killRange = 1f;
    public float minAudioGap = 2f;
    public float maxAudioGap = 10f;
    public AudioClip[] soundEffects;
    public AudioClip[] attackEffects;
    [Header("State Links")]
    public StateType onTargetLostStateChange = StateType.Alert;

    #endregion
    #region Private value holders 

    WaitForSeconds waitFor;
    Collider target;
    private float untilNextSound;
    bool killed = false;

    #endregion
    #region Unity callbacks
    #endregion
    #region Interface implementation

    public string stateName { get; } = "Hostile";
    public StateType stateType { get; } = StateType.Hostile;
    public void Entry(object[] data = null)
    {
        if (GetComponent<NavMeshAgent>().enabled) GetComponent<NavMeshAgent>().speed = movementSpeed;
        GetComponent<Vision>().spotAngle = spotAngle;
        GetComponent<Vision>().range = visionRange;
        GetComponent<Vision>().peripheralVisionRange = peripheralVisionRange;
        GetComponent<Hearing>().range = hearingRange;

        untilNextSound = 0.1f;

        target = data[0] as Collider;
        SetDestinationToTarget();
    }
    public object[] Exit()
    {
        return new object[2] { target, true };
    }
    public IEnumerator StateProcess()
    {
        if (TargetSense() != target)
        {
            GetComponent<Agent>().ChangeState(onTargetLostStateChange);
        }
        else
        {
            KillRangeCheck();
            SetDestinationToTarget();
        }

        if (soundEffects.Length > 0)
        {
            if (untilNextSound <= 0f)
            {
                int soundEffectIndex = Random.Range(0, soundEffects.Length);
                GetComponent<AudioSource>().clip = soundEffects[soundEffectIndex];
                if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();

                untilNextSound = Random.Range(minAudioGap, maxAudioGap);
            }
            untilNextSound -= Time.deltaTime;
        }
        yield return waitFor = new WaitForSeconds(1f / executesPerSecond);
    }

    #endregion
    #region Methods

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
        return sensedTarget;
    }
    void KillRangeCheck()
    {
        if (Vector3.Distance(target.transform.position, transform.position) <= killRange && !killed)
        {
            if (target.tag == "Player")
            {
                SendMessage("OnAttackPlayer", target, SendMessageOptions.DontRequireReceiver);
                if (attackEffects.Length > 0)
                {
                    transform.GetChild(1).GetComponent<AudioSource>().clip = attackEffects[Random.Range(0, attackEffects.Length)];
                    transform.GetChild(1).GetComponent<AudioSource>().Play();
                    killed = true;
                }
                //Application.Quit();
            }
        }
    }

    #endregion
}
