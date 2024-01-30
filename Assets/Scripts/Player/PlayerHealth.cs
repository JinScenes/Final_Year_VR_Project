using UnityEngine;

namespace VR.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [Range(0f, 200f)] public float playerHealth;

        public void ApplyDamage(float damage)
        {
            playerHealth -= damage;
        }
    }
}
