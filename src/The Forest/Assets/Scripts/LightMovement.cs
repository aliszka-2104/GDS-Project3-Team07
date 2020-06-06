using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    private Vector3 rotation;

    public void SetRotation(float rotation)
    {
        this.rotation = new Vector3(0,0,rotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = rotation;
    }
}
