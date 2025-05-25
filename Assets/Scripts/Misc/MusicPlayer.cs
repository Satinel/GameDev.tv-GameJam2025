using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // [SerializeField] AudioClip _introClip;
    // [SerializeField] AudioClip _mainSong;
    [SerializeField] AudioSource _mainAudioSource;//, _introAudioSource;
    // [SerializeField] float _volume = 0.75f;
    // [SerializeField] bool _isBattleMusic = true, _isBossMusic, _isVictoryMusic;
    [SerializeField] AudioClip _dungeonClip, _regularBattleClip, _eliteBattleClip, _bossBattleClip, _victoryClip, _defeatClip;
    
    // bool _isTiming = false;
    // float _timer = 0;
    // float _introLength = 0;

    // void Start()
    // {
    //     if(!_isBattleMusic && !_isVictoryMusic)
    //     {
    //         StartMusic();
    //     }
    // }

    void OnEnable()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
    }

    void OnDisable()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyKilled -= Enemy_OnEnemyKilled;
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
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
            _mainAudioSource.clip = _eliteBattleClip;
        }
        else
        {
            _mainAudioSource.clip = _regularBattleClip;
        }

        _mainAudioSource.Play();
    }

    void Enemy_OnEnemyKilled(Enemy enemy)
    {
        _mainAudioSource.Stop();
        _mainAudioSource.PlayOneShot(_victoryClip);
    }

    void PlayerCombat_OnCombatResolved()
    {
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

    // void Update()
    // {
    //     if(!_isTiming) { return; }

    //     _timer += Time.unscaledDeltaTime;

    //     if(_timer >= _introLength)
    //     {
    //         PlayMainSong();
    //         _isTiming = false;
    //     }
    // }

    // void StartMusic()
    // {
    //     if(_mainSong && _mainAudioSource)
    //     {
    //         _mainAudioSource.volume = 0;
    //         _mainAudioSource.clip = _mainSong;
    //         _mainAudioSource.loop = !_isVictoryMusic;
    //         _mainAudioSource.Play();
    //         if(_introClip && _introAudioSource)
    //         {
    //             _mainAudioSource.Pause();
    //             _introAudioSource.volume = _volume;
    //             Invoke(nameof(SyncIntro), 0.25f);
    //         }
    //         else
    //         {
    //             _mainAudioSource.volume = _volume;
    //         }
    //     }
    // }

    // void StopMusic()
    // {
    //     _timer = 0;
    //     _isTiming = false;
    //     _introAudioSource.Stop();
    //     _mainAudioSource.Stop();
    // }

    // void SyncIntro()
    // {
    //     _introAudioSource.PlayOneShot(_introClip, _volume);
    //     StartTimer(_introClip.length); // This works by ignoring Time.timeScale in Update() HOWEVER it's far from seamless even in editor and is also inconsistent in timing
    // }

    // void StartTimer(float introL)
    // {
    //     _isTiming = true;
    //     _timer = 0;
    //     _introLength = introL;
    // }

    // void PlayMainSong()
    // {
        // _mainAudioSource.volume = _volume;
        // _mainAudioSource.UnPause();
    // }

}
