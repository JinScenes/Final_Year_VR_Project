using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderColourChange : MonoBehaviour
{
    public Slider slider;
    public Image fillArea;

    private void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        slider.onValueChanged.AddListener(UpdateFillColor);
    }

    private void UpdateFillColor(float value)
    {
        float percentage = value / slider.maxValue;

        if (percentage > 0.66f)
        {
            fillArea.color = Color.yellow;
        }
        else if (percentage < 0.33f)
        {
            fillArea.color = Color.red;
        }
        else
        {
            fillArea.color = Color.green;
        }
    }
}
