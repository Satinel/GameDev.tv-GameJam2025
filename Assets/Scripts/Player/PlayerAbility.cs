using UnityEngine;
using System;

public class PlayerAbility : MonoBehaviour
{
    public static event Action<string> OnPlayerAbilityUsed;

    [field:SerializeField] public string Name { get; protected set; } = "Generic Ability";
    [field:SerializeField] public string Adjective { get; protected set; } = "<color=Green>Player</color>";
    [field:SerializeField] public float HitChance { get; protected set; } = 75f;
    [field:SerializeField] public int Damage { get; protected set; } = 10;
    [field:SerializeField] public bool AlwaysHits { get; protected set; } = false;
    [field:SerializeField] public bool DealsDamage { get; protected set; } = true;
    [field:SerializeField] public string Description { get; protected set; }
    [field:SerializeField] public AnimationClip HitAnimation { get; protected set; }
    [field:SerializeField] public AnimationClip MissAnimation { get; protected set; }

    [SerializeField] string UseMessage = string.Empty;
    [SerializeField] bool _sendUseMessage;

    public virtual void Hit()
    {
        if(_sendUseMessage)
        {
            OnPlayerAbilityUsed?.Invoke(UseMessage);
        }
        // Cause status ailment or what-have-you
    }

    public virtual void Miss()
    {
        // This is just for the Tear ability
    }
}
