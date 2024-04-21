using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    int activeSlotIndexNum = 0;
    PlayerControls playerControls;

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void Start() {
        playerControls.Inventory.Keyboard.performed += ContextMenu => ToggleActiveSlot((int)ContextMenu.ReadValue<float>());
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue-1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;
        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
    }
}
