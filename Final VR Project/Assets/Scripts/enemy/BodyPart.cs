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
        if (partHealth <= 0f && Limb != null)
        {
            // Process the body part destruction or damage effect here
            hitBodyPart();
        }

        // Always notify the central health script, regardless of part health
        // This way, damage is centralized and can be modified easily
        zombieHealth.TakeDamage(amount, this);
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
