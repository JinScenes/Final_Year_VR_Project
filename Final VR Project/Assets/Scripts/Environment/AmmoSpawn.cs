using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawn : MonoBehaviour
{
    [SerializeField] private GameObject ammoPrefab;
    private List<Transform> spawnLocations = new List<Transform>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            spawnLocations.Add(child);
        }
    }

    public void SpawnAmmo()
    {
        foreach (Transform spawnLocation in spawnLocations)
        {
            if (Random.value > 0.5f)
            {
                Instantiate(ammoPrefab, spawnLocation.position, Quaternion.identity, spawnLocation);
                //print("Ammo box spawned at " + spawnLocation.name);
            }
            else
            {
                //print("No ammo box spawned at " + spawnLocation.name);
            }
        }
    }

}
