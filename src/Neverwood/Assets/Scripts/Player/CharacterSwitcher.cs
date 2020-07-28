using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    private PlayerMovement[] characters;
    private int currentCharacter = 0;

    void Start()
    {
        characters = FindObjectsOfType<PlayerMovement>();
        characters[currentCharacter].SetCurrentCharacter(true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCharacter();
        }
    }

    private void SwitchCharacter()
    {
        characters[currentCharacter].SetCurrentCharacter(false);

        currentCharacter++;
        currentCharacter %= characters.Length;

        characters[currentCharacter].SetCurrentCharacter(true);
    }
}
