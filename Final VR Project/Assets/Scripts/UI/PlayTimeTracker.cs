using UnityEngine;
using UnityEngine.UI;

public class PlayTimeTracker : MonoBehaviour
{
    [SerializeField] private Text playTimeText;
    private float playTime = 0f;

    void Update()
    {
        playTime += Time.deltaTime;

        int hours = (int)(playTime / 3600);
        int minutes = (int)((playTime % 3600) / 60);
        int seconds = (int)(playTime % 60);

        if (playTimeText != null)
        {
            playTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }
}
