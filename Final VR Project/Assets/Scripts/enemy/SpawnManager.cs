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

    void Start()
    {
        //vendingMachine.SetActive(false);
        StartNextWave();
    }

    void UpdateZombieCountText()
    {
        zombieCountText.text = "Zombies Left: " + totalZombiesAlive.ToString();
    }

    public void ZombieKilled()
    {
        totalZombiesAlive--;
        UpdateZombieCountText();
        if (totalZombiesAlive <= 0)
        {
            StartCoroutine(WaitAndStartNextWave());
        }
    }

    IEnumerator SpawnZombies()
    {
        for (int i = 0; i < totalZombiesToSpawn; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject zombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)]; // Randomly select a zombie prefab
            Instantiate(zombiePrefab, spawnPoint.transform.position, Quaternion.identity);
            totalZombiesAlive++;
            yield return new WaitForSeconds(1f); // spawn interval
        }
        UpdateZombieCountText();
    }

    IEnumerator WaitAndStartNextWave()
    {
        vendingMachine.SetActive(true);
        yield return new WaitForSeconds(60); // time between waves
        vendingMachine.SetActive(false);
        StartNextWave();
    }

    void StartNextWave()
    {
        waveNumber++;
        totalZombiesToSpawn = waveNumber * 5; // example formula for increasing zombie count each wave
        StartCoroutine(SpawnZombies());
    }
}
