using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private CharacterController charController;
    public GameObject audioCue;
    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Vector3 movementVector = new Vector3(0, 0, 0);
        int horAxis = Input.GetAxis("Horizontal").CompareTo(0);
        int verAxis = Input.GetAxis("Vertical").CompareTo(0);
        movementVector.x = horAxis;
        movementVector.z = verAxis;
        charController.Move(movementVector * movementSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space))                                         //For a dummy stun, to be deleted
        {
            GameObject a = Instantiate(audioCue) as GameObject;
            a.GetComponent<AuditoryCue>().range = 8f;
            a.GetComponent<AuditoryCue>().length = 3f;
            a.transform.position = transform.position;
        }
    }
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
        #else
        Application.Quit();
        #endif
    }
}
