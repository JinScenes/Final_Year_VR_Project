using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gamemanager : MonoBehaviour
{


    public float health;
    [SerializeField] GameObject death;
    [SerializeField] GameObject Hui;
    
    public float maxHealth = 0;

    public void Awake()
    {
        health = 100;
        maxHealth =+ health;
    }

    
    public void Update()
    {

        if(health <= 0)
        {
            Time.timeScale = 0f;
            Hui.SetActive(false);
            //death.SetActive(true);
        }

    }
    
}
