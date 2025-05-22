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
    }

    void OnDestroy()
    {
        PlayerHealth.OnHealthChanged -= PlayerHealth_OnHealthChanged;
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
        _slider.value = (float)current / max;
        _text.text = $"HP {current.FormatLargeNumbers()}/{max.FormatLargeNumbers()}";
    }
}
