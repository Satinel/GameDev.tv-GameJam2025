using System;
using UnityEngine;

public class EnemyAbility : MonoBehaviour
{
    public static event Action<EnemyAbility> OnEnemyAbilityStarted;
    public static event Action<EnemyAbility> OnEnemyAbilityUsed;
    [field:SerializeField] public string Name { get; private set; } = "Attack";
    [field:SerializeField] public string Adjective { get; private set; } = "Normal";
    [field:SerializeField] public float HitChance { get; private set; } = 75f;
    [field:SerializeField] public int Damage { get; private set; } = 10;
    [field:SerializeField] public bool AlwaysHits { get; private set; } = false;
    [field:SerializeField] public bool IsOpener { get; private set; } = false;
    [field:SerializeField] public bool IsSingleUse { get; private set; } = false;
    [field:SerializeField] public bool DealsDamage { get; private set; } = true;
    [field:SerializeField] public Color Color1 { get; private set; } = Color.white;
    [field:SerializeField] public Color Color2 { get; private set; } = Color.red;
    [field:SerializeField] public Color ColorFont { get; private set; } = Color.black;

    public virtual void StartAbility()
    {
        OnEnemyAbilityStarted?.Invoke(this);
    }

    public virtual void UseAbility()
    {
        OnEnemyAbilityUsed?.Invoke(this);
    }

    public virtual void Hit()
    {
        // Cause status ailment or what-have-you
    }
}
