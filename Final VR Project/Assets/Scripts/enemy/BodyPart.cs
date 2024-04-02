using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] BodyPart[] parts;
    [SerializeField] GameObject Limb;
    [SerializeField] float partHealth;
    [SerializeField] private GameObject headVFX; // Assign the VFX prefab for the head in the inspector
    [SerializeField] private bool isHeadPart;

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
        if (isHeadPart)
        {
            if (headVFX != null)
            {
                Instantiate(headVFX, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Head VFX prefab is not assigned.");
            }
        }

        if (parts.Length > 0)
        {
            foreach (BodyPart part in parts)
            {
                if (part != null)
                {
                    part.hitBodyPart();
                }
            }
        }

        if (Limb != null)
        {
            GameObject newLimb = Instantiate(Limb, transform.position, transform.rotation);
            newLimb.AddComponent<Rigidbody>();
            newLimb.AddComponent<BoxCollider>();
        }

        gameObject.SetActive(false);
        Destroy(this);
    }

}
