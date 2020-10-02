using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Text ammoText;
    public Text keysText;

    private Inventory inventory;
    private Door door;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        door = FindObjectOfType<Door>();
    }

    private void Start()
    {
        inventory.InventoryUpdate += OnInventoryUpdate;
        if (!door) keysText.transform.parent.gameObject.SetActive(false);
        OnInventoryUpdate();
    }

    void OnInventoryUpdate()
    {
        if (door) keysText.text = inventory.GetItemCount(3).ToString() + "/" + door.keysNeeded.ToString();
        ammoText.text = inventory.GetItemCount(0).ToString();
    }
}
