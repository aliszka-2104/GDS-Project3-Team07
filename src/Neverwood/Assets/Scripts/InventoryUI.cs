using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Text ammoText;

    private Inventory inventory;
    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        inventory.InventoryUpdate += OnInventoryUpdate;
    }

    void OnInventoryUpdate()
    {
        ammoText.text = inventory.GetItemCount(0).ToString();
    }
}
