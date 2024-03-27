using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class WeaponSlot:MonoBehaviour
{
    [SerializeField]
    private List<WeaponEntry> weaponEntries;
    public Dictionary<string, WeaponDetails> weaponDict = new Dictionary<string, WeaponDetails>();
    public string selectedWeaponName;
    [HideInInspector] // Hide in Inspector since we set it at runtime.
    public WeaponDetails selectedWeaponDetails;// The name of the currently selected weapon

    // Call this method to pick a random weapon and update the mesh
    
    void Awake()
    {
        // Populate the dictionary from the serialized list on Awake
        weaponDict = new Dictionary<string, WeaponDetails>();
        foreach (WeaponEntry entry in weaponEntries)
        {
            weaponDict.Add(entry.weaponName, entry.weaponDetails);
        }

        RandomizeWeapon();
    }
    // Call this to deactivate the weapon slot and hide all weapon meshes
    public void HideWeapons()
    {
        foreach (var weaponDetail in weaponDict.Values)
        {
            weaponDetail.weaponMesh.SetActive(false);
        }
    }

    public int GetPrice()
    {
        if (weaponDict.TryGetValue(selectedWeaponName, out WeaponDetails details))
        {
            return details.price;
        }
        return -1; // Return -1 or some other value to indicate that the price wasn't found
    }
    public void RandomizeWeapon()
    {
        // This is called at Awake, but you can call it again to randomize as needed.
        List<string> keys = new List<string>(weaponDict.Keys);
        string randomKey = keys[Random.Range(0, keys.Count)];
        selectedWeaponDetails = weaponDict[randomKey];
        selectedWeaponDetails.weaponMesh.SetActive(true);
    }


}


[System.Serializable]
public class WeaponDetails
{
    public GameObject weaponMesh;
    public int price;

    public WeaponDetails(GameObject mesh, int weaponPrice)
    {
        weaponMesh = mesh;
        price = weaponPrice;
    }
}

[System.Serializable]
public class WeaponEntry
{
    public string weaponName;
    public WeaponDetails weaponDetails;

    public WeaponEntry(string name, WeaponDetails details)
    {
        weaponName = name;
        weaponDetails = details;
    }
}

