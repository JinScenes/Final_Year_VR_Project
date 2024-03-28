using UnityEngine;

public class Magazine : MonoBehaviour
{
    private float cooldownTime = 3f;
    private float lastInteractionTime = -10f;

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time > lastInteractionTime + cooldownTime)
        {
            AmmoDispenser dispenser = other.GetComponentInParent<AmmoDispenser>();
            if (dispenser != null)
            {
                lastInteractionTime = Time.time; // Capture the time at which interaction occurs
                Debug.Log($"Interaction occurred. New lastInteractionTime: {lastInteractionTime}");
                dispenser.AddAmmo(gameObject.name);
                Destroy(gameObject); // Destroys the magazine object
            }
        }
        else
        {
            Debug.Log("Interaction skipped due to cooldown.");
        }
    }

}
