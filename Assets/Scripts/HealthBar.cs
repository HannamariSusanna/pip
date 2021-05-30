using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void SetMaxHealth(int energy) {
        slider.maxValue = energy;
        slider.value = energy;
    }

    public void SetHealth(int energy) {
        slider.value = energy;
    }

    public void Disabled() {
        fill.color = Color.white;
    }

    public void Enabled() {
        fill.color = Color.red;
    }
}
