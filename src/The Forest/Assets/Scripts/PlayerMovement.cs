using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 1;

    public bool isCurrentCharacter = false;

    private Rigidbody2D rigidbody;
    private PlayerIsometricDirection playerIsometricDirection;
    private LightMovement lightMovement;
    private PlayerEnemyInteraction playerEnemyInteraction;
    private Vector2 myMovement = Vector2.zero;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerIsometricDirection = GetComponent<PlayerIsometricDirection>();
        lightMovement = GetComponentInChildren<LightMovement>();
        playerEnemyInteraction = GetComponent<PlayerEnemyInteraction>();
    }

    private void FixedUpdate()
    {
        if (!isCurrentCharacter)
        {
            playerIsometricDirection.SetDirection(Vector2.zero);
            return;
        }

        var currentPosition = rigidbody.position;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var input = new Vector2(horizontal, vertical);
        input = Vector2.ClampMagnitude(input, 1);
        var movement = input * speed;
        var newPosition = currentPosition + movement * Time.deltaTime;

        myMovement = movement;
        rigidbody.MovePosition(newPosition);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerIsometricDirection.SetDirection(myMovement);

        if (myMovement.magnitude > 0.1f) lightMovement.SetRotation(playerIsometricDirection.GetIndex(myMovement) * 45f);
        playerEnemyInteraction.Raycast(myMovement);
    }
}
