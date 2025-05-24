using UnityEngine;
using TMPro;

public class CombatLog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _log;

    void Awake()
    {
        SpikedCarapace.OnActivated += SpikedCarapace_OnActivated;
        PoisonBuffsStrength.OnActivated += AddActivationToLog;
        TacticalLens.OnActivated += AddActivationToLog;
        PlayerHealth.OnPlayerRevive += PlayerHealth_OnPlayerRevive;
        AblativeShell.OnActivated += AddActivationToLog;
        ParalyzingVenom.OnActivated += ParalyzingVenom_OnActivated;
        DiningFork.OnActivated += DiningFork_OnActivated;
        DrinkStraw.OnActivated += DrinkStraw_OnActivated;
        MidasPincer.OnActivated += MidasPincer_OnActivated;
        PlayerCombat.OnRerollUsed += PlayerCombat_OnRerollUsed;
    }

    void OnDestroy()
    {
        SpikedCarapace.OnActivated -= SpikedCarapace_OnActivated;
        PoisonBuffsStrength.OnActivated -= AddActivationToLog;
        TacticalLens.OnActivated -= AddActivationToLog;
        PlayerHealth.OnPlayerRevive -= PlayerHealth_OnPlayerRevive;
        AblativeShell.OnActivated -= AddActivationToLog;
        ParalyzingVenom.OnActivated -= ParalyzingVenom_OnActivated;
        DiningFork.OnActivated -= DiningFork_OnActivated;
        DrinkStraw.OnActivated -= DrinkStraw_OnActivated;
        MidasPincer.OnActivated -= MidasPincer_OnActivated;
        PlayerCombat.OnRerollUsed -= PlayerCombat_OnRerollUsed;
    }

    void AddToLog(string message)
    {
        _log.text += message;
    }

    void AddActivationToLog(string name)
    {
        AddToLog($"\n{name}\nActivated!\n");
    }

    void SpikedCarapace_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"{amount.FormatLargeNumbers()} Retaliation Damage!\n");
    }

    void PlayerHealth_OnPlayerRevive(Trinket reviveTrinket, int health)
    {
        AddToLog($"\n{reviveTrinket.Name}\nActivated!\n{health.FormatLargeNumbers()} Restored!\n");
    }

    void ParalyzingVenom_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Reduced Evasion By {amount.FormatLargeNumbers()}\n");
    }

    void DiningFork_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Gained {amount.FormatLargeNumbers()} Health!");
    }

    void DrinkStraw_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Gained {amount.FormatLargeNumbers()} Health!");
    }

    void MidasPincer_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Gained {amount.FormatLargeNumbers()} Bug Bucks!");
    }

    void PlayerCombat_OnRerollUsed(Trinket trinket)
    {
        AddToLog($"\nMiss!");
        AddActivationToLog(trinket.Name);
    }
}
