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
            UpdateWeaponSlotUI(slotUI.textMeshProUGUI, slotUI.weaponSlotIndex);
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
            int price = slot.GetPrice(); // Make sure you have a method to get the price
            if (CreditsManager.Instance.CanAfford(price))
            {
                CreditsManager.Instance.SpendCredits(price);
                slot.VendWeapon(spawner, spawnerAnimation);
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


    private void UpdateWeaponSlotUI(TextMeshProUGUI slotText, int index)
    {
        if (index < 0 || index >= weaponSlots.Length || slotText == null)
        {
            Debug.LogError("Invalid index or TextMeshProUGUI for updating weapon slot UI.");
            return;
        }

        WeaponSlot slot = weaponSlots[index];
        if (slot != null)
        {
            // Here we use GetPrice() instead of a non-existent property
            int price = slot.GetPrice();
            if (price != -1)
            {
                // Update the TextMeshProUGUI with weapon name and price
                slotText.text = slot.selectedWeaponName + "\n$" + price.ToString();
            }
            else
            {
                Debug.LogError("Could not get price for weapon slot at index " + index);
            }
        }
        else
        {
            Debug.LogError("Weapon Slot at index " + index + " is not assigned.");
        }
    }
}