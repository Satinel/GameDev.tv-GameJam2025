using UnityEngine;

public class Trinket : MonoBehaviour
{
    [SerializeField] protected string _startingName;
    [SerializeField] protected string _toolTipText;

    public string Name { get; private set; }
    public int Level { get; private set; } = 0;

    protected virtual void Start()
    {
        Name = _startingName;
    }

    public virtual void LevelUp()
    {
        Level++;
        Name = $"{_startingName} (+{Level})";
    }

    protected virtual void Activation()
    {
        // Each Trinket will call this under unique circumstances to do a unique thing
    }
}
