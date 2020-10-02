using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    public bool IsCurrentCharacter { get; set; }
    private CinemachineVirtualCamera myCamera;

    void Awake()
    {
        myCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        SetCurrentCharacter(false);
    }

    public void TakeHit()
    {
        SendMessage("OnTakeHit");
        
    }


    public void SetCurrentCharacter(bool isCurrent)
    {
        IsCurrentCharacter = isCurrent;
        if (isCurrent)
        {
            myCamera.Priority=10;
            return;
        }
        myCamera.Priority=0;
    }
}
