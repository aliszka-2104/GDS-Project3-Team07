using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameObject GameOverScreen;
    private GameObject VictoryScreen;
    private GameObject InGameScreen;

    private void Awake()
    {
        instance = this;
        GameOverScreen = GameObject.Find("Game over");
        VictoryScreen = GameObject.Find("Victory");
        InGameScreen = GameObject.Find("In game");
    }

    private void Start()
    {
        GameOverScreen.SetActive(false);
        VictoryScreen.SetActive(false);
        InGameScreen.SetActive(true);
    }

    public void ResetLevel()
    {
        StartCoroutine("Reset");
    }

    public void LoadLevel(string name)
    {
        StartCoroutine("Load",name);
    }

    private IEnumerator Load(string name)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(name);
    }

    private IEnumerator Reset()
    {
        ShowGameOverScreen();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShowGameOverScreen()
    {
        GameOverScreen.SetActive(true);
    }
    private void ShowVictoryScreen()
    {
        VictoryScreen.SetActive(true);
    }
}
