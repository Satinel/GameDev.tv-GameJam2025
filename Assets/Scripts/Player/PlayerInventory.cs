using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool HasKey { get; private set; }

    public void GetKey()
    {
        HasKey = true;
    }
}
