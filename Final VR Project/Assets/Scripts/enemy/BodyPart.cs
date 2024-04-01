using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] BodyPart[] parts;
    [SerializeField] GameObject Limb;
    [SerializeField] float partHealth;
    private ZombieHealth zombieHealth;
    void Start()
    {
        zombieHealth = GetComponentInParent<ZombieHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float amount)
    {
        partHealth -= amount;
        if (partHealth <= 0f)
        {
            // Part-specific logic
            if (Limb != null)
            {
                hitBodyPart();
            }
            // Notify the central health script
            zombieHealth.TakeDamage(amount);
        }
    }



    public void hitBodyPart()
    { 

        if(parts.Length > 0) {
        
                foreach(BodyPart part in parts) 
                    { 
            
                    if(part != null)
                        {
                    part.hitBodyPart();
                        }

                    }

        }

        if(Limb != null)
        {
            GameObject NewLimb = Instantiate(Limb, transform.position, transform.rotation);
           NewLimb.AddComponent<Rigidbody>();
            NewLimb.AddComponent<BoxCollider>();
        }
                transform.localScale = Vector3.zero;
                  
                Destroy(this);
    }
}
