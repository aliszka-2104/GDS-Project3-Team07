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

    private void Awake()
    {
        //lantern.GetComponent<IntroLantern>().LanternHit += OnLanternHit;
        lantern.GetComponent<IntroLantern>().LanternPickedUp += OnLanternPickedUp;
        playerLantern = FindObjectOfType<Lantern>();
        boyAnimator=FindObjectOfType<BoyAnimator>();
        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    void Start()
    {
        lanternStates = boyAnimator.lanternStates;
        playerLantern.gameObject.SetActive(false);
        boyAnimator.lanternStates = new float[] { -1 };
        boyAnimator.UpdateLanternState();
        //playerAttack.layerNames = new string[] { "Ground", "NPC", "Lantern" };
    }

    //void OnLanternHit()
    //{
    //    playerAttack.layerNames = new string[] { "Ground", "NPC" };
    //}

    void OnLanternPickedUp()
    {
        boyAnimator.lanternStates = lanternStates;
        boyAnimator.UpdateLanternState();
        playerLantern.gameObject.SetActive(true);
        Destroy(lantern);
        Destroy(this);
    }


}
