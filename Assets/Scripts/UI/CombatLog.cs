using UnityEngine;
using TMPro;

public class CombatLog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _log;

    void Awake()
    {
        Goal.OnKeyClaimed += Goal_OnKeyClaimed;
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
        QuickMolt.OnActivated += QuickMolt_OnActivated;
        NutCracker.OnActivated += NutCracker_OnActivated;
        SharpeningStone.OnActivated += AddActivationToLog;
        Spinneret.OnActivated += Spinneret_OnActivated;
        MushroomCap.OnActivated += MushroomCap_OnActivated;
        PlayerAbilityPoison.OnDamageIncrease += PlayerAbilityPoison_OnDamageIncrease;
    }

    void OnDestroy()
    {
        Goal.OnKeyClaimed += Goal_OnKeyClaimed;
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
        QuickMolt.OnActivated -= QuickMolt_OnActivated;
        NutCracker.OnActivated -= NutCracker_OnActivated;
        SharpeningStone.OnActivated -= AddActivationToLog;
        Spinneret.OnActivated -= Spinneret_OnActivated;
        MushroomCap.OnActivated -= MushroomCap_OnActivated;
        PlayerAbilityPoison.OnDamageIncrease -= PlayerAbilityPoison_OnDamageIncrease;
    }

    void AddToLog(string message)
    {
        _log.text += message;
    }

    void AddActivationToLog(string name)
    {
        AddToLog($"\n{name}\nActivated!\n");
    }

    void Goal_OnKeyClaimed()
    {
        AddToLog($"\nFloor Boss Unlocked!\n");
    }

    void SpikedCarapace_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"{amount.FormatLargeNumbers()} Retaliation Damage!\n");
    }

    void PlayerHealth_OnPlayerRevive(Trinket reviveTrinket, int health)
    {
        AddToLog($"\n{reviveTrinket.Name}\nActivated!\n{health.FormatLargeNumbers()} HP Restored!\n");
    }

    void ParalyzingVenom_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Reduced Evasion By {amount.FormatLargeNumbers()}\n");
    }

    void DiningFork_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Gained {amount.FormatLargeNumbers()} Health!\n");
    }

    void DrinkStraw_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Gained {amount.FormatLargeNumbers()} Health!\n");
    }

    void MidasPincer_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Gained {amount.FormatLargeNumbers()} Bug Bucks!\n");
    }

    void PlayerCombat_OnRerollUsed(Trinket trinket)
    {
        AddToLog($"\nMiss!");
        AddActivationToLog(trinket.Name);
    }

    void QuickMolt_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Gained {amount.FormatLargeNumbers()} Health!\n");
    }

    void NutCracker_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Reduced Fortitude By {amount.FormatLargeNumbers()}\n");
    }

    void Spinneret_OnActivated(string name, int amount)
    {
        AddActivationToLog(name);
        AddToLog($"Reduced Evasion By {amount.FormatLargeNumbers()}\n");
    }

    void MushroomCap_OnActivated(string name, int _)
    {
        AddActivationToLog(name);
    }

    void PlayerAbilityPoison_OnDamageIncrease(int totalDamage)
    {
        AddToLog($"Poison Damage Raised to {totalDamage.FormatLargeNumbers()}");
    }
}
