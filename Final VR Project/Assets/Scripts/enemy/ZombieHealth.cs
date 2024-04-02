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
        // You can add a death animation or effect here
        GetComponent<EnemyDism>().StartRagdoll(); // This will start the ragdoll effect
        Destroy(gameObject, 5f); // Remove the zombie object
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
