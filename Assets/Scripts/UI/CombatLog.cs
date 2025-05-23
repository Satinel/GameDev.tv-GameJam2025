using UnityEngine;
using TMPro;

public class CombatLog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _log;

    void Awake()
    {
        SpikedCarapace.OnActivated += SpikedCarapace_OnActivated;
    }

    void OnDestroy()
    {
        SpikedCarapace.OnActivated -= SpikedCarapace_OnActivated;
    }

    void AddToLog(string message)
    {
        _log.text += message;
    }

    void SpikedCarapace_OnActivated(int amount)
    {
        AddToLog($"\nSpiked Carapace Activated!\n{amount.FormatLargeNumbers()} Retaliation Damage!\n");
    }
}
