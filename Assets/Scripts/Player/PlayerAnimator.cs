using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator _leftAnimator, _rightAnimator, _tailAnimator;

    void Awake()
    {
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        Enemy.OnEnemyKilled += Enemy_OnAnyEnemyKilled;
        PlayerCombat.OnPlayerAbilityHit += PlayerCombat_OnPlayerAbilityHit;
        PlayerCombat.OnPlayerAbilityMiss += PlayerCombat_OnPlayerAbilityMiss;
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        Enemy.OnEnemyKilled -= Enemy_OnAnyEnemyKilled;
        PlayerCombat.OnPlayerAbilityHit -= PlayerCombat_OnPlayerAbilityHit;
        PlayerCombat.OnPlayerAbilityMiss -= PlayerCombat_OnPlayerAbilityMiss;
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
        _leftAnimator.SetBool("IsMovingt", false);
        _rightAnimator.SetBool("InCombat", true);
        _rightAnimator.SetBool("IsMovingt", false);
        _tailAnimator.SetBool("InCombat", true);
        _tailAnimator.SetBool("IsMovingt", false);
    }

    void Enemy_OnAnyEnemyKilled(Enemy enemy)
    {
        _leftAnimator.SetBool("InCombat", false);
        _rightAnimator.SetBool("InCombat", false);
        _tailAnimator.SetBool("InCombat", false);
    }

    void PlayerCombat_OnPlayerAbilityHit(int index)
    {
        switch(index)
        {
            case 0:
                _leftAnimator.SetTrigger("Attack1");
                break;
            case 1:
                _leftAnimator.SetTrigger("Attack2");
                break;
            case 2:
                _tailAnimator.SetTrigger("Attack1");
                break;
            case 3:
                _tailAnimator.SetTrigger("Attack2");
                break;
            case 4:
                _rightAnimator.SetTrigger("Attack1");
                break;
            case 5:
                _rightAnimator.SetTrigger("Attack2");
                break;
            default:
                _leftAnimator.SetTrigger("Attack1");
                break;
        }
    }

    void PlayerCombat_OnPlayerAbilityMiss(int index)
    {
        switch(index)
        {
            case 0:
                _leftAnimator.SetTrigger("Miss");
                break;
            case 1:
                _leftAnimator.SetTrigger("Miss");
                break;
            case 2:
                _tailAnimator.SetTrigger("Miss");
                break;
            case 3:
                _tailAnimator.SetTrigger("Miss");
                break;
            case 4:
                _rightAnimator.SetTrigger("Miss");
                break;
            case 5:
                _rightAnimator.SetTrigger("Miss");
                break;
            default:
                _leftAnimator.SetTrigger("Miss");
                break;
        }
    }
}
