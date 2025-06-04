using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CombatLog : MonoBehaviour
{
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] TextMeshProUGUI _log;
    [SerializeField] TextMeshProUGUI _tipText;
    [SerializeField] GameObject _toolTip;
    [SerializeField] int _maxLineCount = 100;
    bool _inCombat;
    float _timer;
    [SerializeField] float _tipDuration = 0.45f;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _trinketSFX, _restAreaSFX;

    void Awake()
    {
        Enemy.OnFightStarted += Enemy_OnFightStarted;
        PlayerCombat.OnCombatResolved += PlayerCombat_OnCombatResolved;
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
        Enemy.OnVenomStrengthened += Enemy_OnVenomStrengthened;
        PlayerStats.OnBonusXPEarned += PlayerStats_OnBonusXPEarned;
        PlayerStats.OnStatIncreased += PlayerStats_OnStatIncreased;
        PlayerInventory.OnTrinketAdded += PlayerInventory_OnTrinketAdded;
        PlayerInventory.OnTrinketLevelled += PlayerInventory_OnTrinketLevelled;
        RestAreaUI.OnRestAreaUsed += RestAreaUI_OnRestAreaUsed;
    }

    void OnDestroy()
    {
        Enemy.OnFightStarted -= Enemy_OnFightStarted;
        PlayerCombat.OnCombatResolved -= PlayerCombat_OnCombatResolved;
        Goal.OnKeyClaimed -= Goal_OnKeyClaimed;
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
        Enemy.OnVenomStrengthened -= Enemy_OnVenomStrengthened;
        PlayerStats.OnBonusXPEarned -= PlayerStats_OnBonusXPEarned;
        PlayerStats.OnStatIncreased -= PlayerStats_OnStatIncreased;
        PlayerInventory.OnTrinketAdded -= PlayerInventory_OnTrinketAdded;
        PlayerInventory.OnTrinketLevelled -= PlayerInventory_OnTrinketLevelled;
        RestAreaUI.OnRestAreaUsed -= RestAreaUI_OnRestAreaUsed;
    }

    void Update()
    {
        if(!_inCombat) { return; }

        if(_timer < _tipDuration)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _toolTip.SetActive(false);
        }
    }

    void AddToLog(string message)
    {
        _log.text += message;
    }

    void AddActivationToLog(string name)
    {
        if(_inCombat)
        {
            // if(_audioSource && _trinketSFX)
            // {
            //     _audioSource.PlayOneShot(_trinketSFX);
            // }
            _tipText.text = $"{name}\nActivated!\n";
            _toolTip.SetActive(true);
            _timer = 0;
        }
        AddToLog($"\n{name}\nActivated!\n");
    }

    void Enemy_OnFightStarted(Enemy _)
    {
        _inCombat = true;
        _scrollRect.verticalNormalizedPosition = 0;
    }

    void PlayerCombat_OnCombatResolved()
    {
        _inCombat = false;
        _toolTip.SetActive(false);
        _timer = 0;

        if(_log.textInfo.lineCount > _maxLineCount + 1)
        {
            string[] lines = _log.text.Split('\n');
            List<string> trimmedLines = new();
            int startIndex = lines.Length - _maxLineCount;
            for (int i = startIndex; i < lines.Length - 1; i++)
            {
                trimmedLines.Add(lines[i]);
            }
            _log.text = string.Empty;
            foreach(string line in trimmedLines)
            {
                _log.text += $"\n{line}";
            }
        }
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
        AddToLog($"Gained {amount.FormatLargeNumbers()} BugBucks!\n");
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
        AddToLog($"Venom Damage Raised To {totalDamage.FormatLargeNumbers()}\n");
    }

    void Enemy_OnVenomStrengthened(int amount)
    {
        AddToLog($"Venom Damage\nIncresed By {amount.FormatLargeNumbers()}\n");
    }

    void PlayerStats_OnBonusXPEarned(int amount)
    {
        AddToLog($"Earned {amount.FormatLargeNumbers()} Bonus XP!\n");
    }

    void PlayerStats_OnStatIncreased(PlayerStats.Stats stat, int amount)
    {
        AddToLog($"\n{stat} Increased By {amount.FormatLargeNumbers()}!\n");
    }

    void PlayerInventory_OnTrinketAdded(Trinket trinket)
    {
        AddToLog($"\nAcquired {trinket.StartingName}!\n");
    }

    void PlayerInventory_OnTrinketLevelled(Trinket trinket)
    {
        AddToLog($"\nUpgraded {trinket.StartingName} to +{trinket.Level}!\n");
    }

    void RestAreaUI_OnRestAreaUsed()
    {
        AddToLog($"\nYou're well rested!\nFully Recovered HP!\n");
        if(_audioSource && _restAreaSFX)
        {
            _audioSource.PlayOneShot(_restAreaSFX);
        }
    }
}
