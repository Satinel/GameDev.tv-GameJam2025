using UnityEngine;
using System;

public class PlayerAbility : MonoBehaviour
{
    public static event Action<PlayerAbility> OnPlayerAbilityStarted;
    public static event Action<PlayerAbility> OnPlayerAbilityUsed;
    [field:SerializeField] public string Name { get; private set; } = "Generic Ability";
    [field:SerializeField] public string Adjective { get; private set; } = "<color=Green>Player</color>";
    [field:SerializeField] public float HitChance { get; private set; } = 75f;
    [field:SerializeField] public int Damage { get; private set; } = 10;
    [field:SerializeField] public bool AlwaysHits { get; private set; } = false;
    [field:SerializeField] public bool DealsDamage { get; private set; } = true;

    public virtual void StartAbility()
    {
        OnPlayerAbilityStarted?.Invoke(this);
    }

    public virtual void UseAbility()
    {
        OnPlayerAbilityUsed?.Invoke(this);
    }

    public virtual void Hit()
    {
        // Cause status ailment or what-have-you
    }
}
