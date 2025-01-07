using UnityEngine;
using UnityEngine.UI;

public class HealthBarBoss : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(int currentValue, int maxValue)
    {
        if (slider == null)
        {
            Debug.LogError("Slider is not assigned in the Inspector!");
            return;
        }

        slider.value = (float)currentValue / maxValue; // Update slider value
    }
}