using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public GameObject[] EnemyPrefab;

    float spwn = 0f;

    int ran;

    int randomNum;
    int putValue;
    float spwnr8 = 5;

    int randomEnemy;
    // Start is called before the first frame update
    void Start()
    {
        SpawnNewEnemy(0);
    }

    // Update is called once per frame
    void Update()
    {
        spwn += Time.deltaTime;
        if(spwn >= spwnr8)
        {

            RandomSpawn();
            spwn = 0;
            
        }
        //print(spwn);
        
    }
    void SpawnNewEnemy(int spawnpoint)
    {
        // if (gameObject.tag == "Player"


         randomEnemy = Random.Range(1, 6);

        switch (randomEnemy)
        {
            case 5:
                Instantiate(EnemyPrefab[0], SpawnPoints[spawnpoint].transform.position, Quaternion.identity);
                break;
            case 4:
                Instantiate(EnemyPrefab[1], SpawnPoints[spawnpoint].transform.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(EnemyPrefab[2], SpawnPoints[spawnpoint].transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(EnemyPrefab[3], SpawnPoints[spawnpoint].transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(EnemyPrefab[4], SpawnPoints[spawnpoint].transform.position, Quaternion.identity);
                break;

        }
    }

    void RandomSpawn()
    {
        ran = Random.Range(1, 6);

        print(ran);

        switch (ran)
        {
            case 5:
                SpawnNewEnemy(0);
                break;
            case 4:
                SpawnNewEnemy(1);
                break;
            case 3:
                SpawnNewEnemy(2);
                break;
            case 2:
                SpawnNewEnemy(3);
                break;
            case 1:
                SpawnNewEnemy(4);
                break;
            
        }
    }
    void RandomEnemySpawn()
    {
    



    }
}
