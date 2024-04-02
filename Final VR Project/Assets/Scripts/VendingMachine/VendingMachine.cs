using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

[System.Serializable] // This makes WeaponSlotUI visible in the inspector.
public class WeaponSlotUI
{
    public TextMeshProUGUI textMeshProUGUI;
    public int weaponSlotIndex;
}

public class VendingMachine : MonoBehaviour
{
    public WeaponSlot[] weaponSlots; // Your existing WeaponSlot array
    public GameObject spawner; // Assign this in the Inspector
    public Animation spawnerAnimation; // Assign this in the Inspector
    public WeaponSlotUI[] weaponSlotUIs; // Array for setting up in the inspector

    private Dictionary<TextMeshProUGUI, int> slotTextMapping;

    void Start()
    {
        InitializeSlotTextMapping();

        foreach (WeaponSlotUI slotUI in weaponSlotUIs)
        {
            // Ensure the weapon slot index is within bounds
            if (slotUI.weaponSlotIndex < 0 || slotUI.weaponSlotIndex >= weaponSlots.Length)
            {
                Debug.LogError($"WeaponSlotUI at index {Array.IndexOf(weaponSlotUIs, slotUI)} has an out-of-range weaponSlotIndex of {slotUI.weaponSlotIndex}.");
                continue;
            }

            // Proceed to hide weapons and randomize
            weaponSlots[slotUI.weaponSlotIndex].HideWeapons();
            weaponSlots[slotUI.weaponSlotIndex].RandomizeWeapon();
            UpdateUIForSlot(slotUI.textMeshProUGUI, slotUI.weaponSlotIndex);
        }
    }



    private void InitializeSlotTextMapping()
    {
        slotTextMapping = new Dictionary<TextMeshProUGUI, int>();

        foreach (WeaponSlotUI slotUI in weaponSlotUIs)
        {
            if (slotUI.textMeshProUGUI != null)
            {
                slotTextMapping[slotUI.textMeshProUGUI] = slotUI.weaponSlotIndex;
            }
            else
            {
                Debug.LogError("One of the TextMeshProUGUI components is not assigned in the inspector.");
            }
        }
    }

    public void VendWeapon(int index)
    {
        if (index < 0 || index >= weaponSlots.Length)
        {
            Debug.LogError("Invalid slot index.");
            return;
        }

        WeaponSlot slot = weaponSlots[index];
        if (slot != null)
        {
            int price = slot.GetPrice();
            print(price);
            if (CreditsManager.Instance.Credits >= slot.GetPrice())
            {
                CreditsManager.Instance.SpendCredits(price);
                slot.VendWeapon(spawner);
                Debug.Log("Dispensed Weapon: " + slot.selectedWeaponName);

                // Update UI here as needed
            }
            else
            {
                Debug.Log("Not enough credits to purchase.");
            }
        }
        else
        {
            Debug.LogError("Weapon Slot at index " + index + " is not assigned.");
        }
    }

    public static VendingMachine Instance;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateUIForSlot(WeaponSlot slot)
    {
        // Find the UI element associated with this slot and update it
        foreach (var slotUI in weaponSlotUIs)
        {
            if (slotUI.weaponSlotIndex == Array.IndexOf(weaponSlots, slot))
            {
                slotUI.textMeshProUGUI.text = $"{slot.selectedWeaponName}\n${slot.GetPrice()}";
                break;
            }
        }
    }

    
}
