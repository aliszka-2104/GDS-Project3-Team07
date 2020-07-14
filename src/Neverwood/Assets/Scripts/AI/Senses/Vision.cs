using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vision : Sense
{
    #region Sense Parameters

    public float spotAngle = 45f;
    public float peripheralVisionRange = 1f;

    #endregion
    #region Private value holders

    private const string senseName = "Vision";
    public override string SenseName
    {
        get
        {
            return senseName;
        }
    }

    #endregion
    #region Unity callbacks

    #endregion
    #region Methods

    public override bool PreciseSense(Collider vaguelySensed)
    {
        bool preciselySensed = false;                                                                                                          //Result storing
        Vector3 forwardVector = transform.forward;
        Vector3 currentPosition = transform.position + centerOffset;
        Vector3 colliderPosition = vaguelySensed.transform.position;
        Vector3 vectorToCollider = colliderPosition - currentPosition;
        Collider[] collsInPeripheralRange = Physics.OverlapSphere(currentPosition, peripheralVisionRange, senseMask);

        bool isInPeripheralRange = collsInPeripheralRange.Contains(vaguelySensed);                                                             //Check if the object is within the peripheral range
        bool isInVisionCone = Vector3.Angle(forwardVector, vectorToCollider) < spotAngle / 2;                                                  //Check if the object is within the vision cone
        if (isInPeripheralRange || isInVisionCone)
        {
            float distanceToCollider = Vector3.Distance(currentPosition, colliderPosition);
            bool inLineOfSight = !Physics.Raycast(currentPosition, vectorToCollider, distanceToCollider, senseBlockingMask);
            if (inLineOfSight)
            {
                preciselySensed = true;
            }
        }
        return preciselySensed;
    }
    public bool IsColliderInSight(Collider collider)
    {
        foreach(Collider elem in UseSense())
        {
            if(elem == collider)
            {
                return true;
            }
        }
        return false;
    }

    #endregion
    #region Debug

    [Header("Debug")]
    //DEBUG
    public bool DEBUGActive = true;
    public Color DEBUGColor = Color.red;
    private void OnDrawGizmos()
    {
        if (DEBUGActive)
        {
            Vector3 currentPosition = transform.position + centerOffset;
            Vector3 forwardVector = transform.forward * range;
            Gizmos.color = DEBUGColor.linear;
            Gizmos.DrawRay(currentPosition, forwardVector);
            Gizmos.DrawRay(currentPosition, Quaternion.Euler(0, spotAngle / 2, 0) * forwardVector);
            Gizmos.DrawRay(currentPosition, Quaternion.Euler(0, -spotAngle / 2, 0) * forwardVector);
            Gizmos.DrawWireSphere(currentPosition, peripheralVisionRange);
        }
    }

    #endregion
}
