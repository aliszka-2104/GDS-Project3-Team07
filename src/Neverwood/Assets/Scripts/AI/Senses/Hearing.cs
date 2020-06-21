using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : Sense
{
    #region Sense Parameters


    #endregion
    #region Private value holders

    private const string senseName = "Hearing";
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
        return true;
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

            Gizmos.color = DEBUGColor.linear - new Color(0, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(currentPosition, range);
        }
    }

    #endregion
}
