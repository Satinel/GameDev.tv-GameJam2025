using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator _leftAnimator, _rightAnimator, _tailAnimator;
    [SerializeField] Animator _leftShadow, _rightShadow, _tailShadow;

    void Awake()
    {
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyKilled += Enemy_OnAnyEnemyKilled;
        PlayerCombat.OnPlayerAbilityUsed += PlayerCombat_OnPlayerAbilityUsed;
        // PlayerCombat.OnPlayerAbilityMiss += PlayerCombat_OnPlayerAbilityMiss;
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyKilled -= Enemy_OnAnyEnemyKilled;
        PlayerCombat.OnPlayerAbilityUsed -= PlayerCombat_OnPlayerAbilityUsed;
        // PlayerCombat.OnPlayerAbilityMiss -= PlayerCombat_OnPlayerAbilityMiss;
    }

    void PlayerHealth_OnPlayerDeath()
    {
        _leftAnimator.enabled = false;
        _rightAnimator.enabled = false;
        _tailAnimator.enabled = false;
    }

    void Enemy_OnFightStarted(Enemy enemy)
    {
        _leftAnimator.SetBool("InCombat", true);
        _leftAnimator.SetBool("IsMoving", false);
        _rightAnimator.SetBool("InCombat", true);
        _rightAnimator.SetBool("IsMoving", false);
        _tailAnimator.SetBool("InCombat", true);
        _tailAnimator.SetBool("IsMoving", false);
        _leftShadow.SetBool("InCombat", true);
        _leftShadow.SetBool("IsMoving", false);
        _rightShadow.SetBool("InCombat", true);
        _rightShadow.SetBool("IsMoving", false);
        _tailShadow.SetBool("InCombat", true);
        _tailShadow.SetBool("IsMoving", false);
    }

    void Enemy_OnAnyEnemyKilled(Enemy enemy)
    {
        _leftAnimator.SetBool("InCombat", false);
        _rightAnimator.SetBool("InCombat", false);
        _tailAnimator.SetBool("InCombat", false);
        _leftShadow.SetBool("InCombat", false);
        _rightShadow.SetBool("InCombat", false);
        _tailShadow.SetBool("InCombat", false);
    }

    void PlayerCombat_OnPlayerAbilityUsed(int index, string clip)
    {
        switch(index)
        {
            case 0:
                _leftAnimator.Play(clip);
                _leftShadow.Play(clip);
                break;
            case 1:
                _leftAnimator.Play(clip);
                _leftShadow.Play(clip);
                break;
            case 2:
                _tailAnimator.Play(clip);
                _tailShadow.Play(clip);
                break;
            case 3:
                _tailAnimator.Play(clip);
                _tailShadow.Play(clip);
                break;
            case 4:
                _rightAnimator.Play(clip);
                _rightShadow.Play(clip);
                break;
            case 5:
                _rightAnimator.Play(clip);
                _rightShadow.Play(clip);
                break;
            default:
                _leftAnimator.Play(clip);
                _leftShadow.Play(clip);
                break;
        }
    }

    /*void PlayerCombat_OnPlayerAbilityMiss(int index, string clip)
    {
        switch(index)
        {
            case 0:
                _leftAnimator.SetTrigger("Miss1");
                _leftShadow.SetTrigger("Miss1");
                break;
            case 1:
                _leftAnimator.SetTrigger("Miss2");
                _leftShadow.SetTrigger("Miss2");
                break;
            case 2:
                _tailAnimator.SetTrigger("Miss1");
                _tailShadow.SetTrigger("Miss1");
                break;
            case 3:
                _tailAnimator.SetTrigger("Miss2");
                _tailShadow.SetTrigger("Miss2");
                break;
            case 4:
                _rightAnimator.SetTrigger("Miss1");
                _rightShadow.SetTrigger("Miss1");
                break;
            case 5:
                _rightAnimator.SetTrigger("Miss2");
                _rightShadow.SetTrigger("Miss2");
                break;
            default:
                _leftAnimator.SetTrigger("Miss1");
                _leftShadow.SetTrigger("Miss1");
                break;
        }
    }*/
}
