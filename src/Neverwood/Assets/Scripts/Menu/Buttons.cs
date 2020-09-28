using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    GameObject creditsFrame;

    private void Awake()
    {
        creditsFrame = GameObject.Find("CreditsFrame");
        creditsFrame.SetActive(false);
    }
    public void StartButton()
    {
        SceneManager.LoadSceneAsync("Intro");
    }
    public void CreditsButton()
    {
        creditsFrame.SetActive(true);
    }
    public void CreditsBackButton()
    {
        creditsFrame.SetActive(false);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
