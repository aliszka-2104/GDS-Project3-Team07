using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1;

    private Rigidbody2D rigidbody;
    private PlayerIsometricDirection playerIsometricDirection;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerIsometricDirection = GetComponent<PlayerIsometricDirection>();
    }

    private void FixedUpdate()
    {
        var currentPosition = rigidbody.position;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var input = new Vector2(horizontal, vertical);
        input = Vector2.ClampMagnitude(input, 1);
        var movement = input * speed;
        var newPosition = currentPosition + movement * Time.deltaTime;

        rigidbody.MovePosition(newPosition);
        playerIsometricDirection.SetDirection(movement);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
