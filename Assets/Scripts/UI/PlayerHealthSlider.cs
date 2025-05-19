using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealthSlider : MonoBehaviour
{
    public static event Action OnSliderEnabled;

    [SerializeField] TextMeshProUGUI _text;
    Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        PlayerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
        OnSliderEnabled?.Invoke();
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= PlayerHealth_OnHealthChanged;
    }

    void PlayerHealth_OnHealthChanged(int current, int max)
    {
        _slider.maxValue = max;
        _slider.value = current;
        _text.text = $"HP {FormatValueForText(current)}/{FormatValueForText(max)}";
    }

    string FormatValueForText(int value)
    {
        return value switch
        {
            < 1000 => value.ToString(),
            < 10000 => (value / 1000f).ToString("N2") + "K",
            < 100000 => (value / 1000f).ToString("N1") + "K",
            < 1000000 => (value / 100000f).ToString("N2") + "M",
            < 10000000 => (value / 100000f).ToString("N1") + "M",
            < 100000000 => (value / 100000f).ToString("N0") + "M",
            _ => value.ToString(),
        };
    }
}
