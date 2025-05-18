using System;
using UnityEngine;

public class MazeSpace : MazeUnit
{
    public static event Action<Vector2> OnAnySpaceEntered;

    bool _wasEntered;

    void OnTriggerEnter(Collider other)
    {
        if(_wasEntered) { return; }

        if(other.gameObject.GetComponentInParent<PlayerHealth>())
        {
            OnAnySpaceEntered?.Invoke(_coordinates);
            _wasEntered = true;
            Reveal();
        }
    }
}
