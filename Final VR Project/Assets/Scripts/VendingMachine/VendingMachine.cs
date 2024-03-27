using System.Collections;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    public WeaponSlot[] weaponSlots;
    public GameObject spawner; // Assign this in the Inspector
    public Animation spawnerAnimation; // Assign this in the Inspector

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
            slot.VendWeapon(spawner, spawnerAnimation);
            print("Dispensed Weapon: " + slot.selectedWeaponName);
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
            slot.HideWeapons();
            slot.RandomizeWeapon();
        }
    }
}
