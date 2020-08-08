using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public static CharacterSwitcher instance;
    public Action onCharacterSwitch;

    private Player[] characters;
    private int currentCharacter = 0;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        characters = FindObjectsOfType<Player>();
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
        onCharacterSwitch?.Invoke();

        characters[currentCharacter].SetCurrentCharacter(false);

        currentCharacter++;
        currentCharacter %= characters.Length;

        characters[currentCharacter].SetCurrentCharacter(true);
    }
}
