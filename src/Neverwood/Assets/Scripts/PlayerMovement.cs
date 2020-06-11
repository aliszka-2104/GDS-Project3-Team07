using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private CharacterController charController;
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
    }
}
