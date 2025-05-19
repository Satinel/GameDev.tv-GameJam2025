using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [field:SerializeField] public int Strength { get; set; }
    [field:SerializeField] public int Accuracy { get; set; }
    [field:SerializeField] public int Fortitude { get; set; }
    [field:SerializeField] public int Evasion { get; set; }
    [field:SerializeField] public int Tenacity { get; set; }
    [field:SerializeField] public int Initiative { get; set; }

}
