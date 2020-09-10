using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsCurrentCharacter { get; set; }
    private Camera myCamera;

    void Awake()
    {
        myCamera = GetComponentInChildren<Camera>();
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
            myCamera.gameObject.SetActive(true);
            return;
        }
        myCamera.gameObject.SetActive(false);
    }
}
