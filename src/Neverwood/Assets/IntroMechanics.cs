using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMechanics : MonoBehaviour
{
    public GameObject lantern;

    private BoyAnimator boyAnimator;
    private float[] lanternStates;

    private PlayerAttack playerAttack;
    private Lantern playerLantern;

    void Start()
    {
        playerLantern = CharacterSwitcher.instance.boy.GetComponentInChildren<Lantern>();
        playerLantern.gameObject.SetActive(false);

        lantern.GetComponent<IntroLantern>().LanternHit += OnLanternHit;
        lantern.GetComponent<IntroLantern>().LanternPickedUp += OnLanternPickedUp;

        boyAnimator=CharacterSwitcher.instance.boy.GetComponent<BoyAnimator>();
        lanternStates = boyAnimator.lanternStates;
        boyAnimator.lanternStates = new float[] {-1};
        boyAnimator.UpdateLanternState();

        playerAttack = CharacterSwitcher.instance.girl.GetComponent<PlayerAttack>();
        playerAttack.layerNames = new string[]{ "Ground", "NPC","Lantern"};
    }

    void OnLanternHit()
    {
        playerAttack.layerNames = new string[] { "Ground", "NPC" };
    }

    void OnLanternPickedUp()
    {
        boyAnimator.lanternStates = lanternStates;
        boyAnimator.UpdateLanternState();
        playerLantern.gameObject.SetActive(true);
        Destroy(lantern);
        Destroy(this);
    }


}
