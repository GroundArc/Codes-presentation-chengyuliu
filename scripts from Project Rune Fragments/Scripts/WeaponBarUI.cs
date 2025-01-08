using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class WeaponBarUI : MonoBehaviour
{
    public Image[] weaponSlots;
    private KeyCode[] keys;
    public Color SelectedColor;
    public Color UnselectedColor;

    public PlayerInventory playerInventory;
    private int selectedSlot = -1;
    private void Start()
    {
        keys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };
        SelectSlot(0);
        playerInventory = PlayerInventory.Instance;
        if (playerInventory != null && playerInventory.onEnvyAbilityUsed != null)
        {
            playerInventory.onEnvyAbilityUsed.AddListener(UpdateWeaponBar);
        }
        if (playerInventory != null && playerInventory.onBossWeaponCollected != null)
        {
            playerInventory.onBossWeaponCollected.AddListener(UpdateWeaponBar);
        }
        if (playerInventory != null && playerInventory.OnAmmoExhausted != null)
        {
            playerInventory.OnAmmoExhausted.AddListener(UpdateWeaponBar);
        }
    }


    private void Update()
    {
        for (int i = 0; i < keys.Length && i < weaponSlots.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]) && weaponSlots[i].gameObject.activeSelf)
            {
                SelectSlot(i);
            }
        }
    }

    public void SelectSlot(int slot)
    {
        if (selectedSlot != slot)
        {
            if (IsValidSlot(selectedSlot))
            {
                weaponSlots[selectedSlot].color = UnselectedColor;
            }

            selectedSlot = slot;
            if (IsValidSlot(selectedSlot))
            {
                weaponSlots[selectedSlot].color = SelectedColor;
            }
        }
    }

    private bool IsValidSlot(int index)
    {
        return index >= 0 && index < weaponSlots.Length;
    }

    public void UpdateWeaponBar()
    {
        SelectSlot(playerInventory.currentWeaponIndex);
    }
}



