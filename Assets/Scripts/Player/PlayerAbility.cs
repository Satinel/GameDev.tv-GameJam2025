using UnityEngine;
using System;

public class PlayerAbility : MonoBehaviour
{
    public static event Action<string> OnPlayerAbilityUsed;

    [field:SerializeField] public string Name { get; private set; } = "Generic Ability";
    [field:SerializeField] public string Adjective { get; private set; } = "<color=Green>Player</color>";
    [field:SerializeField] public float HitChance { get; private set; } = 75f;
    [field:SerializeField] public int Damage { get; private set; } = 10;
    [field:SerializeField] public bool AlwaysHits { get; private set; } = false;
    [field:SerializeField] public bool DealsDamage { get; private set; } = true;

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
}
