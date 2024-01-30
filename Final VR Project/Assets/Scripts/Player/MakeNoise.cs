using UnityEngine;

namespace VR.Player
{
    public class MakeNoise : MonoBehaviour
    {
        [SerializeField] KeyCode NoiseKey;
        
        [HideInInspector] public bool Noise;
        float timer;

        void Update()
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
            }

            if (Input.GetKeyDown(NoiseKey))
            {
                Noise = true;
                timer = 0.1f;
            }

            if (Noise == true && timer <= 0.0f)
            {
                Noise = false;
            }
        }
    }
}
