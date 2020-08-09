using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum OBSTACLE
    {
        ROCK,
        LOG
    }

    public Transform target;
    public OBSTACLE obstacleType;
    public GameObject effects;

    private void Start()
    {
        CharacterSwitcher.instance.onCharacterSwitch += OnCharacterSwitch;
    }

    private void OnCharacterSwitch()
    {
        if (effects!=null)
        {
            effects.SetActive(!effects.activeSelf); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerMovement>()) return;

        other.GetComponent<PlayerMovement>().CrossObstacle(target.position, obstacleType);
    }

    private void OnDestroy()
    {
        CharacterSwitcher.instance.onCharacterSwitch -= OnCharacterSwitch;
    }
}
