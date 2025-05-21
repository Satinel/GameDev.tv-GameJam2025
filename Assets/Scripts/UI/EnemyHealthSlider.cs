using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthSlider : MonoBehaviour
{
    [SerializeField] Slider _slider, _gradualSlider;
    [SerializeField] TextMeshProUGUI _text, _nameText;
        [SerializeField] float _gradualSpeed = 0.25f;

    Enemy _currentEnemy;

    void Start()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyHealthChanged += Enemy_OnEnemyHealthChanged;
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
    }

    void OnDestroy()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyHealthChanged -= Enemy_OnEnemyHealthChanged;
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
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


    void Enemy_OnFightStarted(Enemy enemy)
    {
        _currentEnemy = enemy;
        _slider.value = _currentEnemy.CurrentHealth / (float)_currentEnemy.MaxHealth;
        _gradualSlider.value = _slider.value;
        _text.text = $"HP {_currentEnemy.CurrentHealth.FormatLargeNumbers()}/{_currentEnemy.MaxHealth.FormatLargeNumbers()}";
        _nameText.text = _currentEnemy.Name;
        _slider.enabled = true;
    }

    void Enemy_OnEnemyHealthChanged(int amount)
    {
        _slider.value = _currentEnemy.CurrentHealth / (float)_currentEnemy.MaxHealth;
        
        _text.text = $"HP {_currentEnemy.CurrentHealth.FormatLargeNumbers()}/{_currentEnemy.MaxHealth.FormatLargeNumbers()}";
    }

    void PlayerCombat_OnCombatResolved()
    {
        _nameText.text = string.Empty;
        _slider.enabled = false;
    }
}
