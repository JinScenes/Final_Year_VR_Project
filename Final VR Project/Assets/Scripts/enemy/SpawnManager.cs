using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] zombiePrefabs; // Array of different zombie prefabs
    public GameObject vendingMachine;
    public TextMeshProUGUI zombieCountText;
    private int totalZombiesToSpawn;
    private int totalZombiesAlive;
    private int waveNumber = 0;
    private bool canSpawn = false;

    void Start()
    {
        
        // StartNextWave();
    }

    void UpdateZombieCountText()
    {
        zombieCountText.text = "Zombies Left: " + totalZombiesAlive.ToString();
    }

    public void ZombieKilled()
    {
        if (totalZombiesAlive > 0)
        {
            totalZombiesAlive--;
            UpdateZombieCountText();
        }

        if (totalZombiesAlive <= 0)
        {
            StartCoroutine(WaitAndStartNextWave());
        }
    }

    IEnumerator SpawnZombies()
    {
        canSpawn = true; 
        for (int i = 0; i < totalZombiesToSpawn && canSpawn; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject zombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)]; 
            Instantiate(zombiePrefab, spawnPoint.transform.position, Quaternion.identity);
            totalZombiesAlive++;
            yield return new WaitForSeconds(1f); // spawn interval
        }
        canSpawn = false; // Prevent further spawning after the wave's zombies are all spawned
        UpdateZombieCountText();
    }

    IEnumerator WaitAndStartNextWave()
    {
        vendingMachine.SetActive(true);
        yield return new WaitForSeconds(30); // time between waves
        vendingMachine.SetActive(false);
        StartNextWave();
    }

    public void StartNextWave()
    {
        vendingMachine.SetActive(false);
        waveNumber++;
        totalZombiesToSpawn = waveNumber * 5; // Adjust the formula as needed for your game's difficulty curve
        StartCoroutine(SpawnZombies());
    }
}
