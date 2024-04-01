using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public static CreditsManager Instance { get; private set; }

    public int Credits { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCredits(int amount)
    {
        Credits += amount;
        // Optionally, update UI or notify the player here
    }

    public bool CanAfford(int amount)
    {
        return Credits >= amount;
    }

    public void SpendCredits(int amount)
    {
        if (CanAfford(amount))
        {
            Credits -= amount;
            // Optionally, update UI or notify the player here
        }
        else
        {
            Debug.LogWarning("Not enough credits to spend.");
        }
    }
}
