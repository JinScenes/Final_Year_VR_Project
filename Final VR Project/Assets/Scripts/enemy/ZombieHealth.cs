using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 100f;
    public float totalHealth = 100f;
    private EnemyDism enemyDism;

    void Start()
    {
        enemyDism = GetComponent<EnemyDism>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        FindObjectOfType<SpawnManager>().ZombieKilled();
        
        CreditsManager.Instance.AddCredits(25);
        GetComponent<EnemyDism>().StartRagdoll(); 
        Destroy(gameObject, 5f); 
    }


    public void TakeDamage(float amount, BodyPart hitPart = null)
    {
        if (hitPart != null)
        {
            hitPart.TakeDamage(amount); // You should add a TakeDamage method in BodyPart that reduces its health
        }
        else
        {
            health -= amount;
        }

        if (health <= 0f)
        {
            Die();
        }
    }

}
