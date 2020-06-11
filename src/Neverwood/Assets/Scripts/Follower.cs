using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform player;
    public float speed;
    PathUnit pathUnit;
    float pathUpdateTime = 0.1f;
    float timeLeft;
    private void Awake()
    {
        pathUnit = gameObject.AddComponent<PathUnit>();
        pathUnit.target = player;
        pathUnit.speed = speed;
        timeLeft = pathUpdateTime;
    }
    private void Start()
    {
        pathUnit.GoToTarget();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))           //Prowizoryczne kontrolki
        {
            pathUnit.Stop();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            pathUnit.Come();
        }
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)                                  //Aktualizacja ścieżki co okreslony czas, nie co kratkę - wydajność
        {
            pathUnit.GoToTarget();
            timeLeft = pathUpdateTime;
        }
    }
}
