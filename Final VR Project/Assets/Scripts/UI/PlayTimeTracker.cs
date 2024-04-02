using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class PlayTimeTracker : MonoBehaviour
{
    [SerializeField] private Text playTimeText;
    [SerializeField] private Image notificationImage;

    [SerializeField] private GameObject notificationUI;

    [SerializeField] private int notificationIntervalMinutes = 20;

    public UnityAction OnNotificationTimeReached;
    private Coroutine flashCoroutine;

    private float playTime = 0f;
    private float notificationInterval;

    private bool notificationVisible = false;

    private void Start()
    {
        notificationInterval = notificationIntervalMinutes * 60;
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        UpdatePlayTimeDisplay();

        if (playTime >= notificationInterval && !notificationVisible)
        {
            OnNotificationTimeReached?.Invoke();
            notificationInterval += notificationIntervalMinutes * 60;
        }
    }

    private void UpdatePlayTimeDisplay()
    {
        int hours = (int)(playTime / 3600);
        int minutes = (int)((playTime % 3600) / 60);
        int seconds = (int)(playTime % 60);

        if (playTimeText != null)
        {
            playTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }

    public void ToggleNotificationUI(bool show)
    {
        notificationVisible = show;
        if (notificationUI != null)
        {
            notificationUI.SetActive(show);

            if (show && flashCoroutine == null)
            {
                flashCoroutine = StartCoroutine(FlashImage(notificationImage));
            }
            else if (!show && flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                flashCoroutine = null;
                notificationImage.color = Color.black;
            }
        }
    }

    public bool NotificationReached()
    {
        return playTime >= notificationInterval;
    }

    public bool IsNotificationVisible()
    {
        return notificationVisible;
    }

    public IEnumerator FlashImage(Image image)
    {
        Color originalColor = image.color;

        while (true)
        {
            image.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            image.color = originalColor;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
