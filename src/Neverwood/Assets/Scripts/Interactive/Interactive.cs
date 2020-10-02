using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public virtual void Interact() => Debug.Log("Interact " + this.name);
}
