using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public static CharacterSwitcher instance;
    public Player boy { get; set; }
    public Player girl { get; set; }
    public Player CurrentCharacter
    {
        get
        {
            return characters[currentCharacter];
        }
    }

    public Action onCharacterSwitch;

    private Player[] characters;
    private int currentCharacter = 0;

    private void Awake()
    {
        instance = this;
        boy = GameObject.Find("Boy").GetComponent<Player>();
        girl = GameObject.Find("Girl").GetComponent<Player>();
        characters = new[] { girl, boy };
    }
    void Start()
    {
        characters[currentCharacter].SetCurrentCharacter(true);
    }

    public void OnCharacterSwitch()
    {
        instance.CurrentCharacter.GetComponent<PlayerMovement>().ChangeDirection(Vector2.zero);
        if(Vector3.Distance(boy.transform.position, girl.transform.position) < 3f && CurrentCharacter.OtherPlayer.GetComponent<PlayerMovement>().IsFollowing)
        {
            CurrentCharacter.GetComponent<PlayerMovement>().IsFollowing = true;
        }
        else if (Vector3.Distance(boy.transform.position, girl.transform.position) < 3f && !CurrentCharacter.OtherPlayer.GetComponent<PlayerMovement>().IsFollowing)
        {
            CurrentCharacter.GetComponent<PlayerMovement>().IsFollowing = false;
        }
        else if (Vector3.Distance(boy.transform.position, girl.transform.position) >= 3f)
        {
            CurrentCharacter.GetComponent<PlayerMovement>().IsFollowing = false;
        }
        SwitchCharacter();
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