using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawn : MonoBehaviour
{
    public GameObject[] ammoObjects; 
    private GameObject[] spawnLocations; 

    void Start()
    {
        spawnLocations = GameObject.FindGameObjectsWithTag("AmmoSpawn");
    }

    public void SpawnAmmo()
    {
        foreach (GameObject spawnLocation in spawnLocations)
        {
            foreach (Transform child in spawnLocation.transform)
            {
                if (child.tag == "Ammo")
                {
                    Destroy(child.gameObject);
                }
            }
            GameObject ammoToSpawn = ammoObjects[Random.Range(0, ammoObjects.Length)];
            GameObject spawnedAmmo = Instantiate(ammoToSpawn, spawnLocation.transform.position, Quaternion.identity);
            spawnedAmmo.transform.SetParent(spawnLocation.transform);
            print("Ammo Spawned");
        }
    }
}
