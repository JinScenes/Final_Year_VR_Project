using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.CompareTag("Gun"))
        {
            
            Gun Agun = collision.gameObject.GetComponent<Gun>();
            Agun.ammo = 30;
            Agun.mag.SetActive(true);
        }
    }
}
