using UnityEngine;

public class Equipable : MonoBehaviour
{
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public PlayerAbility Ability1 { get; private set; }
    [field:SerializeField] public PlayerAbility Ability2 { get; private set; }

}
