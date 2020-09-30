using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject GameOverScreen;
    public GameObject VictoryScreen;
    public GameObject InGameScreen;

    public AudioClip[] musicClips;

    private Dictionary<string, AudioClip> levelMusic = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        instance = this;
        //GameOverScreen = GameObject.Find("Game over");
        //VictoryScreen = GameObject.Find("Victory");
        //InGameScreen = GameObject.Find("In game");

        levelMusic.Add("Mainmenu", musicClips[0]);
        levelMusic.Add("Level1", musicClips[1]);
        levelMusic.Add("Level2", musicClips[2]);
    }

    private void Start()
    {
        GameOverScreen.SetActive(false);
        VictoryScreen.SetActive(false);
        InGameScreen.SetActive(true);

        GetComponent<AudioSource>().clip = levelMusic[SceneManager.GetActiveScene().name];
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = true;
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
