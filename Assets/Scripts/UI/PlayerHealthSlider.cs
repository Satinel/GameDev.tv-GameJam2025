using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthSlider : MonoBehaviour
{
    [SerializeField] Slider _slider, _gradualSlider;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _gradualSpeed = 0.25f;

    void Start()
    {
        PlayerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
        PlayerHealth.OnInitialHealthSettings += PlayerHealth_OnInitialHealthSettings;
    }

    void OnDestroy()
    {
        PlayerHealth.OnHealthChanged -= PlayerHealth_OnHealthChanged;
        PlayerHealth.OnInitialHealthSettings -= PlayerHealth_OnInitialHealthSettings;
    }

    void Update()
    {
        if(!_gradualSlider) { return; }

        if(_gradualSlider.value > _slider.value)
        {
            _gradualSlider.value -= _gradualSpeed * Time.deltaTime;
        }
        else
        {
            _gradualSlider.value = _slider.value;
        }
    }

    void PlayerHealth_OnHealthChanged(int current, int max)
    {
        SetHealthValues(current, max);
    }

    void PlayerHealth_OnInitialHealthSettings(int current, int max)
    {
        SetHealthValues(current, max);
        _gradualSlider.value = _slider.value;
    }

    public void SetHealthValues(int current, int max)
    {
        _slider.value = (float)current / max;
        _text.text = $"HP {current.FormatLargeNumbers()}/{max.FormatLargeNumbers()}";
    }
}
