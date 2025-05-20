using UnityEngine;

public class Trinket : MonoBehaviour
{
    [field:SerializeField] public string Name { get; set; }
    public int Level { get; private set; } = 0;

    public void LevelUp()
    {
        Level++;
        Name = $"{Name} (+{Level})";
    }

    protected virtual void OnActivation()
    {
        // Each Trinket will call this under unique circumstances to do a unique thing
    }
}
