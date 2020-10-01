using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public float speed = 5f;
    UnityEngine.UI.Image fadeScreen;

    private void Start()
    {
        CharacterSwitcher.instance.boy.tag = "";
        CharacterSwitcher.instance.girl.tag = "";
        CharacterSwitcher.instance.boy.enabled = false;
        CharacterSwitcher.instance.girl.enabled = false;
        Transform.FindObjectOfType<InputMap>().enabled = false;

        fadeScreen = GameObject.Find("FadeScreen").GetComponent<UnityEngine.UI.Image>();
        GameObject.Find("Canvas").transform.Find("Victory").gameObject.SetActive(true);
        StartCoroutine(GoToMainMenu());
    }

    private void Update()
    {
        CharacterSwitcher.instance.boy.GetComponent<CharacterController>().Move(Vector3.forward * speed * Time.deltaTime);
        CharacterSwitcher.instance.girl.GetComponent<CharacterController>().Move(Vector3.forward * speed * Time.deltaTime);
        fadeScreen.color = fadeScreen.color + new Color(0, 0, 0, speed / 10f * Time.deltaTime);
    }

    IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(6f);
        GameManager.instance.LoadLevel("Mainmenu");
    }
}
