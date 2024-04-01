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
            List<GameObject> objectsToDestroy = new List<GameObject>();

            foreach (Transform child in spawnLocation.transform)
            {
                if (child == null) continue;
                if (child.tag == "Ammo")
                {
                    objectsToDestroy.Add(child.gameObject);
                }

            }
            foreach (GameObject obj in objectsToDestroy)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }

            GameObject ammoToSpawn = ammoObjects[Random.Range(0, ammoObjects.Length)];
            GameObject spawnedAmmo = Instantiate(ammoToSpawn, spawnLocation.transform.position, Quaternion.identity);
            spawnedAmmo.transform.SetParent(spawnLocation.transform);
            print("Ammo Spawned");
        }
    }
}
