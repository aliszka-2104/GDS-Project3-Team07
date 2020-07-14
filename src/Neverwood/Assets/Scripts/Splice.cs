using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Splice : MonoBehaviour
{
    private int previousJointNumber = 0;
#if UNITY_EDITOR
    public int jointNumber = 5;
    GUIStyle numberStyle = new GUIStyle();
    private void OnDrawGizmos()
    {
        numberStyle.fontSize = 24;
        numberStyle.normal.textColor = Color.white;
        if (previousJointNumber == 0) { previousJointNumber = jointNumber; }
        if (transform.childCount == 0)
        {
            for (int i = 0; i < jointNumber; i++)
            {
                Expand();
            }
        }
        if(jointNumber > 0)
        {
            int difference = jointNumber - previousJointNumber;
            if (difference != 0)
            {
                if (difference > 0)
                {
                    for (int i = 0; i < difference; i++)
                    {
                        Expand();
                    }
                }
                else
                {
                    for (int i = 0; i < -difference; i++)
                    {
                        Shrink();
                    }
                }
            }
            previousJointNumber = jointNumber;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.GetChild(0).position, 0.2f);
        Handles.Label(transform.GetChild(0).position, transform.GetChild(0).name, numberStyle);
        for (int i = 1; i < transform.childCount; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.GetChild(i - 1).position, transform.GetChild(i).position);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.GetChild(i).position, 0.2f);
            Handles.Label(transform.GetChild(i).position, transform.GetChild(i).name, numberStyle);
        }
    }

    void Expand()
    {
        GameObject newChild = new GameObject();
        if(transform.childCount != 0)
        {
            newChild.transform.position = transform.GetChild(transform.childCount - 1).position + Vector3.forward;
        }
        else
        {
            newChild.transform.position = transform.position;
        }
        newChild.transform.SetParent(transform);
        newChild.name = transform.childCount.ToString();
    }
    void Shrink()
    {
        DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);
    }
#endif
    private List<Vector3> waypoints = new List<Vector3>();
    public Vector3[] path
    {
        get
        {
            return waypoints.ToArray();
        }
    }
    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            waypoints.Add(transform.GetChild(i).position);
        }
    }
}
