using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource _mainAudioSource;
    [SerializeField] AudioClip _dungeonClip, _regularBattleClip, _eliteBattleClip, _bossBattleClip, _victoryClip, _defeatClip, _bossDefeatClip;
    [SerializeField] AudioClip _storeClip, _alternateBattleClip, _alternateEliteClip;
    bool _bossDefeated;

    void OnEnable()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        Store.OnEnteredStore += Store_OnEnteredStore;
        StoreUI.OnExitStore += StoreUI_OnExitStore;
    }

    void OnDisable()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyKilled -= Enemy_OnEnemyKilled;
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        Store.OnEnteredStore -= Store_OnEnteredStore;
        StoreUI.OnExitStore -= StoreUI_OnExitStore;
    }

    void Enemy_OnFightStarted(Enemy enemy)
    {
        _mainAudioSource.Stop();

        if(enemy.IsBoss)
        {
            _mainAudioSource.clip = _bossBattleClip;
        }
        else if(enemy.IsElite)
        {
            if(_alternateEliteClip && Random.Range(0, 5) > 3)
            {
                _mainAudioSource.clip = _alternateEliteClip;
            }
            else
            {
                _mainAudioSource.clip = _eliteBattleClip;
            }
        }
        else
        {
            if(_alternateBattleClip && Random.Range(0, 5) > 3)
            {
                _mainAudioSource.clip = _alternateBattleClip;
            }
            else
            {
                _mainAudioSource.clip = _regularBattleClip;
            }
        }

        _mainAudioSource.Play();
    }

    void Enemy_OnEnemyKilled(Enemy enemy)
    {
        _mainAudioSource.Stop();
        if(enemy.IsBoss)
        {
            _mainAudioSource.PlayOneShot(_bossDefeatClip);
            _bossDefeated = true;
        }
        else
        {
            _mainAudioSource.PlayOneShot(_victoryClip);
        }
    }

    void PlayerCombat_OnCombatResolved()
    {
        if(_bossDefeated) { return; }

        _mainAudioSource.Stop();
        _mainAudioSource.clip = _dungeonClip;
        _mainAudioSource.Play();
    }

    void PlayerHealth_OnPlayerDeath()
    {
        _mainAudioSource.Stop();
        if(_defeatClip)
        {
            _mainAudioSource.PlayOneShot(_defeatClip);
        }
    }

    void Store_OnEnteredStore(Transform _)
    {
        _mainAudioSource.Stop();
        _mainAudioSource.clip = _storeClip;
        _mainAudioSource.Play();
    }

    void StoreUI_OnExitStore()
    {
        _mainAudioSource.Stop();
        _mainAudioSource.clip = _dungeonClip;
        _mainAudioSource.Play();
    }
}
