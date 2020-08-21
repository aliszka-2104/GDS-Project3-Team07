using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void ResetLevel()
    {
        StartCoroutine("Reset");
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(3);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
