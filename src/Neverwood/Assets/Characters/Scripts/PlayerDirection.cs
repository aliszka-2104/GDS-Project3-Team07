using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    public Sprite[] sprites;

    private string[] directions = {"UP","RIGHT","DOWN","LEFT"};
    private int prevDirection = 0;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetDirection(Vector3 movementDirection)
    {
        int index = 0;
        if (movementDirection.x > 0)
        {
            index= 1;
        }
        else if (movementDirection.x<0)
        {
            index= 3;
        }
        else if (movementDirection.z > 0)
        {
            index= 0;
        }
        else if (movementDirection.z < 0)
        {
            index = 2;
        }
        else
        {
            index = prevDirection;
        }
        prevDirection = index;
        SetSprite(index);
    }

    private void SetSprite(int index)
    {
        sr.sprite = sprites[index];
    }
}
