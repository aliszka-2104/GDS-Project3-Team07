using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    private PlayerMovement[] players;

    private int currentCharacterIndex = 0;

    private void Start()
    {
        players = FindObjectsOfType<PlayerMovement>();
        SetCurrentInPlayerScript(true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SwitchCharacter();
        }
    }

    private void SwitchCharacter()
    {
        SetCurrentInPlayerScript(false);

        currentCharacterIndex++;
        currentCharacterIndex = currentCharacterIndex % players.Length;

        SetCurrentInPlayerScript(true);
    }

    private void SetCurrentInPlayerScript(bool isCurrent)
    {
        players[currentCharacterIndex].isCurrentCharacter = isCurrent;
    }
}
