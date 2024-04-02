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
    [HideInInspector] 
    public WeaponDetails selectedWeaponDetails;
    [SerializeField] private GameObject spawner;

    

    void Awake()
    {
        weaponDict.Clear(); 

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
        return -1; 
    }
    public void RandomizeWeapon()
    {
        int randomIndex = Random.Range(0, weaponEntries.Count);
        WeaponEntry selectedEntry = weaponEntries[randomIndex];

        selectedWeaponName = selectedEntry.weaponName;
        selectedWeaponDetails = selectedEntry.weaponDetails;

       
        HideWeapons();
        selectedWeaponDetails.weaponMesh.SetActive(true);

       
        VendingMachine.Instance.UpdateUIForSlot(this);
    }


    
    public void VendWeapon(GameObject spawner)
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
        details.weaponMesh.SetActive(false); 

        Rigidbody rb = details.weaponMesh.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        StartCoroutine(SpawnWeaponPrefab(details.weaponPrefab, spawner));
    }

    private IEnumerator SpawnWeaponPrefab(GameObject prefab, GameObject spawner)
    {
        yield return new WaitForSeconds(1); 

        
        GameObject spawnedWeapon = Instantiate(prefab, spawner.transform.position, spawner.transform.rotation);

        
        spawnedWeapon.transform.SetParent(spawner.transform);

        
        spawnedWeapon.transform.SetParent(null);

        
        Rigidbody rb = spawnedWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            
            Vector3 forceDirection = -spawner.transform.up; 
            float forceMagnitude = 10.0f; 
            rb.isKinematic = false; 
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.VelocityChange); 
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

