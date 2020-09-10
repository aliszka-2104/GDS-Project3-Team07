using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMap : MonoBehaviour
{
    public Lantern lantern;

    private Vector3 shootPos = Vector3.zero;

    public void OnLight()
    {
        lantern.LanternToggle();
    }
    public void OnInteract()
    {
        Vector3 currentPosition = CharacterSwitcher.instance.CurrentCharacter.transform.position;
        var colliders = Physics.OverlapSphere(currentPosition, 2f, LayerMask.GetMask("Interactive"));
        if (colliders.Length > 0)
        {
            var first = colliders[0];
            if (first.GetComponent<Interactive>())
            {
                first.GetComponent<Interactive>().Interact();
            }
        }
    }
    public void OnAttack()
    {
        Debug.Log(shootPos);
        Player girl = CharacterSwitcher.instance.girl;
        if(girl.IsCurrentCharacter)
        {
            girl.GetComponent<PlayerAttack>().Shoot(shootPos);
        }
    }
    public void OnMove(InputValue value)
    {
        CharacterSwitcher.instance.CurrentCharacter.GetComponent<PlayerMovement>().ChangeDirection(value.Get<Vector2>());
    }
    public void OnAim(InputValue value)
    {
        Vector2 mousePos = value.Get<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Ground", "NPC")))
        {
            shootPos = hit.point;
        }
        else
        {
            shootPos = Vector3.zero;
        }
    }
}
