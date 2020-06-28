using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AgentStunner : MonoBehaviour
{
    #region Debuff parameters

    public float cooldown = 5f;
    public float stunTime = 2f;
    public float range = 2f;
    public LayerMask AgentMask;

    #endregion
    #region Private value holders

    HashSet<Collider> onCooldown = new HashSet<Collider>();
    WaitForSeconds waitFor;

    #endregion
    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, AgentMask);
        foreach(Collider coll in colliders)
        {
            if(coll.GetComponent<Agent>() && !onCooldown.Contains(coll))
            {
                coll.GetComponent<Agent>().Stun(stunTime);
                onCooldown.Add(coll);
                StartCoroutine(Cooldown(cooldown, coll));
            }
        }
    }
    IEnumerator Cooldown(float time, Collider coll)
    {
        yield return waitFor = new WaitForSeconds(time);
        onCooldown.Remove(coll);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
        foreach(Collider coll in onCooldown)
        {
            Handles.color = Color.white;
            Handles.Label(coll.transform.position + Vector3.up * 3 + Vector3.right * 2, "Stun on cooldown");
        }
    }

}
