    using UnityEngine;

    public class ZombieHealth : MonoBehaviour
    {
        public SpawnManager spawnManager;
        public float health = 70f;
        //public float totalHealth = 100f;
        private EnemyDism enemyDism;
        private bool isDead = false;

        void Start()
        {
            enemyDism = GetComponent<EnemyDism>();
        }



    

    void Die()
    {
        if (!isDead)
        {
            isDead = true; // Prevents multiple death processing

            if (spawnManager != null)
            {
                spawnManager.ZombieKilled();
            }

            GetComponent<ZombieAI>().isAlive = false;
            CreditsManager.Instance.AddCredits(25);
            enemyDism.StartRagdoll();
            Destroy(gameObject, 5f);
        }
    }



    public void TakeDamage(float amount, BodyPart hitPart = null)
    {
        // Check if the damage is from a specific body part
        if (hitPart != null)
        {
            // Adjust the amount of damage if needed based on body part
            // For example, head hits could do more damage
            float damageMultiplier = 1.0f; // Default multiplier
                                           // Example: if(hitPart.partType == BodyPartType.Head) damageMultiplier = 2.0f;

            health -= (amount * damageMultiplier);
        }
        else
        {
            // Direct damage to the zombie, not through a body part
            health -= amount;
        }

        if (health <= 0f)
        {
            Die();
        }
    }
}
        

    
