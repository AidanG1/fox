using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [Header("Health Bar Properties")]
    [Tooltip("The slider to display the health bar.")]
    public Slider slider;
    [Tooltip("The gradient to display the health bar.")]
    public Gradient gradient;
    [Tooltip("The fill to display the health bar.")]
    public Image fill;
    public void MaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
