using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Interactive
{
    public GameObject campfireOn;
    public GameObject campfireOff;
    public GameObject effects;

    private bool disabled = false;

    private void Start()
    {
        CharacterSwitcher.instance.onCharacterSwitch += OnCharacterSwitch;
    }

    private void OnCharacterSwitch()
    {
        if (!disabled)
        {
            effects.SetActive(!effects.activeSelf);
        }
    }

    public override void Interact()
    {
        //campfireOn.SetActive(true);
        //campfireOff.SetActive(false);
        if (CharacterSwitcher.instance.boy.IsCurrentCharacter)
        {
            ToggleLight();
        }
    }

    public void ToggleLight()
    {
        campfireOn.SetActive(!campfireOn.activeSelf);
        campfireOff.SetActive(!campfireOff.activeSelf);
    }
}
