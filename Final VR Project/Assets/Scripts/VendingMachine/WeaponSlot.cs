using System.Collections;
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
    [SerializeField] private GameObject spawner;

    // Call this method to pick a random weapon and update the mesh

    void Awake()
    {
        weaponDict.Clear(); // Ensure the dictionary is clear before initializing it.

        foreach (WeaponEntry entry in weaponEntries)
        {
            if (!weaponDict.ContainsKey(entry.weaponName))
            {
                weaponDict.Add(entry.weaponName, entry.weaponDetails);
            }
            else
            {
                Debug.LogError("Duplicate weapon name detected: " + entry.weaponName);
            }
        }

        if (weaponDict.Count != weaponEntries.Count)
        {
            Debug.LogError("Mismatch between weapon entries and dictionary. Check for duplicates or initialization issues.");
        }
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

        if (weaponDict.Count == 0)
        {
            Debug.LogError("weaponDict is empty. Make sure weaponEntries are properly initialized and Awake() has run.");
            return;
        }

        
        if (keys.Count == 0)
        {
            Debug.LogError("No keys found in weaponDict. Make sure weaponEntries are properly initialized.");
            return;
        }

        int randomIndex = Random.Range(0, keys.Count);
        selectedWeaponName = keys[randomIndex];
        //Debug.Log("Randomized weapon name: " + selectedWeaponName);
        //Debug.Log("Selected weapon name: " + selectedWeaponName);
    }

    // Add this to the WeaponSlot class
    public void VendWeapon(GameObject spawner, Animation spawnerAnim)
    {
        foreach (var kvp in weaponDict)
        {
            Debug.Log("Key: " + kvp.Key + ", Value: " + kvp.Value);
        }
        Debug.Log("Attempting to vend weapon with name: " + selectedWeaponName);


        if (string.IsNullOrEmpty(selectedWeaponName) || !weaponDict.ContainsKey(selectedWeaponName))
        {
            Debug.LogError("Selected weapon name is invalid or not in the dictionary.");
            return;
        }

        WeaponDetails details = weaponDict[selectedWeaponName];
        details.weaponMesh.SetActive(false); // Hide the mesh

        Rigidbody rb = details.weaponMesh.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        StartCoroutine(SpawnWeaponPrefab(details.weaponPrefab, spawner));
    }

    private IEnumerator SpawnWeaponPrefab(GameObject prefab, GameObject spawner)
    {
        yield return new WaitForSeconds(1); // Wait for a second, if needed

        // Instantiate the prefab at the spawner's position
        GameObject spawnedWeapon = Instantiate(prefab, spawner.transform.position, spawner.transform.rotation);

        // Optionally, temporarily parent the prefab to the spawner for alignment before ejection
        spawnedWeapon.transform.SetParent(spawner.transform);

        // Play the spawner animation, if any
       

        // Unparent the prefab from the spawner after the animation
        spawnedWeapon.transform.SetParent(null);

        // Now apply a force to the weapon to "shoot" it out from the vending machine
        Rigidbody rb = spawnedWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Assuming the vending machine's bottom is along the negative y-axis
            // Adjust the force direction and magnitude as needed
            Vector3 forceDirection = -spawner.transform.up; // This will shoot the weapon downward
            float forceMagnitude = 10.0f; // Change this value to achieve the desired ejection speed
            rb.isKinematic = false; // Make sure the Rigidbody is not kinematic
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.VelocityChange); // Apply the force
        }
    }
}


[System.Serializable]
public class WeaponDetails
{
    public GameObject weaponMesh;
    public int price;
    public GameObject weaponPrefab;

    public WeaponDetails(GameObject mesh, int weaponPrice, GameObject prefab)
    {
        weaponMesh = mesh;
        price = weaponPrice;
        prefab = weaponPrefab;
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

