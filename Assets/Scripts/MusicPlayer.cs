using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip _introClip;
    [SerializeField] AudioClip _mainSong;
    [SerializeField] AudioSource _mainAudioSource, _introAudioSource;
    [SerializeField] float _volume = 0.75f;
    [SerializeField] bool _isBattleMusic = true, _isBossMusic, _isVictoryMusic;
    
    bool _isTiming = false;
    float _timer = 0;
    float _introLength = 0;

    void Start()
    {
        if(!_isBattleMusic && !_isVictoryMusic)
        {
            StartMusic();
        }
    }

    void OnEnable()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
    }

    void OnDisable()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyKilled -= Enemy_OnEnemyKilled;
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
    }

    void Update()
    {
        if(!_isTiming) { return; }

        _timer += Time.unscaledDeltaTime;

        if(_timer >= _introLength)
        {
            PlayMainSong();
            _isTiming = false;
        }
    }

    void StartMusic()
    {
        if(_mainSong && _mainAudioSource)
        {
            _mainAudioSource.volume = 0;
            _mainAudioSource.clip = _mainSong;
            _mainAudioSource.loop = !_isVictoryMusic;
            _mainAudioSource.Play();
            if(_introClip && _introAudioSource)
            {
                _mainAudioSource.Pause();
                _introAudioSource.volume = _volume;
                Invoke(nameof(SyncIntro), 0.25f);
            }
            else
            {
                _mainAudioSource.volume = _volume;
            }
        }
    }

    void StopMusic()
    {
        _timer = 0;
        _isTiming = false;
        _introAudioSource.Stop();
        _mainAudioSource.Stop();
    }

    void SyncIntro()
    {
        _introAudioSource.PlayOneShot(_introClip, _volume);
        StartTimer(_introClip.length); // This works by ignoring Time.timeScale in Update() HOWEVER it's far from seamless even in editor and is also inconsistent in timing
    }

    void StartTimer(float introL)
    {
        _isTiming = true;
        _timer = 0;
        _introLength = introL;
    }

    void PlayMainSong()
    {
        _mainAudioSource.volume = _volume;
        _mainAudioSource.UnPause();
    }

    void Enemy_OnFightStarted(Enemy enemy)
    {
        if(!_isBattleMusic)
        {
            StopMusic();
        }
        else if(enemy.IsBoss == _isBossMusic)
        {
            StartMusic();
        }
    }

    void Enemy_OnEnemyKilled(Enemy enemy)
    {
        if(_isVictoryMusic)
        {
            StartMusic();
        }
        else
        {
            StopMusic();
        }
    }

    void PlayerCombat_OnCombatResolved()
    {
        if(_isVictoryMusic)
        {
            StopMusic();
            return;
        }
        if(!_isBattleMusic)
        {
            StartMusic();
        }
        else
        {
            StopMusic();
        }
    }
}
