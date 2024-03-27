using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AmmoCountDisplay : MonoBehaviour
{
    [SerializeField] private AmmoDispenser ammoDispenser;
    [SerializeField] private string weaponType;

    private Text displayText;

    void Start()
    {
        displayText = GetComponent<Text>();
        if (ammoDispenser == null)
        {
            Debug.LogError("AmmoDispenser reference not set on " + gameObject.name);
        }
    }

    void Update()
    {
        if (ammoDispenser != null)
        {
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        switch (weaponType)
        {
            case "Pistol":
                displayText.text = "?";
                break;
            case "SCAR":
                displayText.text = $"{ammoDispenser.CurrentSCARClips}";
                break;
            case "M4A4":
                displayText.text = $"{ammoDispenser.CurrentM4A4Clips}";
                break;
            case "AK74U":
                displayText.text = $"{ammoDispenser.CurrentAK74UClip}";
                break;
            case "SIGMCX":
                displayText.text = $"{ammoDispenser.CurrentSIGMCXClip}";
                break;
            case "Leader_50":
                displayText.text = $"{ammoDispenser.CurrentLeader_50Clip}";
                break;
            case "Shotgun":
                displayText.text = $"{ammoDispenser.CurrentShotgunShells}";
                break;
            default:
                displayText.text = "Unknown Weapon Type";
                break;
        }
    }
}
