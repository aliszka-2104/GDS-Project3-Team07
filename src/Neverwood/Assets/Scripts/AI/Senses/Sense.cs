using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.Events;

public abstract class Sense : MonoBehaviour
{
    #region Sense parameters

    [Header("Sense parameters")]
    public float range = 5f;
    public Vector3 centerOffset = new Vector3(0f, 0f, 0f);
    public LayerMask senseMask;
    public LayerMask senseBlockingMask;

    #endregion
    #region Private value holders

    public abstract string SenseName { get; }

    #endregion
    #region Unity Callbacks



    #endregion
    #region Methods

    public Collider[] VagueSense()
    {
        Collider[] collidersSensed = Physics.OverlapSphere(transform.position + centerOffset, range, senseMask);
        return collidersSensed;
    }
    public abstract bool PreciseSense(Collider vaguelySensed);
    public Collider[] UseSense()
    {
        Collider[] colliderSensed = VagueSense();
        List<Collider> preciselySensed = new List<Collider>();
        foreach(Collider coll in colliderSensed)
        {
            if(PreciseSense(coll))
            {
                preciselySensed.Add(coll);
            }
        }
        return preciselySensed.ToArray();
    }

    #endregion
}
