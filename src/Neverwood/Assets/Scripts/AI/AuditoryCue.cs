using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class AuditoryCue : MonoBehaviour
{
    #region Cue parameters

    public float length = 1f;
    public float range = 5f;

    #endregion
    private void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        GetComponent<SphereCollider>().radius = range;
        StartCoroutine(DestroyAfterDelay(length));
    }

    IEnumerator DestroyAfterDelay(float time)
    {
        yield return new WaitForSeconds(length);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
