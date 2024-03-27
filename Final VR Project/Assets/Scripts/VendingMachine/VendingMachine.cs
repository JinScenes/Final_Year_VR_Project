using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    // Start is called before the first frame update
    public WeaponSlot[] weaponSlots;
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
            // Call the method to vend the selected weapon
            slot.HideWeapons(); // First hide all weapons
            slot.RandomizeWeapon(); // Randomly select a new weapon for future use
            print("Dispensed Weapon: " + slot.selectedWeaponName);
            // Add more logic here to actually vend the weapon (e.g., instantiate it, apply effects, etc.)
        }
        else
        {
            Debug.LogError("Weapon Slot at index " + index + " is not assigned.");
        }
    }

    void Start()
    {
        // Initialize each weapon slot with a random weapon
        foreach (WeaponSlot slot in weaponSlots)
        {
            slot.RandomizeWeapon();
        }
    }


}
