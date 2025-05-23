using UnityEngine;
using TMPro;

public class CombatLog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _log;

    void Awake()
    {
        SpikedCarapace.OnActivated += SpikedCarapace_OnActivated;
        PoisonBuffsStrength.OnActivated += PoisonBuffsStrength_OnActivated;
    }

    void OnDestroy()
    {
        SpikedCarapace.OnActivated -= SpikedCarapace_OnActivated;
        PoisonBuffsStrength.OnActivated += PoisonBuffsStrength_OnActivated;
    }

    void AddToLog(string message)
    {
        _log.text += message;
    }

    void SpikedCarapace_OnActivated(string name, int amount)
    {
        AddToLog($"\n{name}\nActivated!\n{amount.FormatLargeNumbers()} Retaliation Damage!\n");
    }

    void PoisonBuffsStrength_OnActivated(string name)
    {
        AddToLog($"\n{name}\nActivated!");
    }
}
