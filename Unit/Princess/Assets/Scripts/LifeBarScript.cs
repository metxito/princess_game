using UnityEngine;
using UnityEngine.UI;

public class LifeBarScript : MonoBehaviour
{
    public Slider slider;
    public Gradient colorBar;
    public Image fillBar;

    public void SetLife(float max, float current) {
        if (max <= 0f)
            max = 1f;
        slider.maxValue = max;
        slider.minValue = 0f;
        slider.value = current;

        fillBar.color = colorBar.Evaluate(slider.value / slider.maxValue);
    }
}
