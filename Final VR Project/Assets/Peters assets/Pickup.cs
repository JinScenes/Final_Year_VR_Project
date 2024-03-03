using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    
    
    
    [SerializeField] Gamemanager gm;
    [SerializeField] Imagec im;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
                gm.health = +10;
                im.UpdateHealthBar();
            
            Destroy(gameObject);
        }
    }
}
