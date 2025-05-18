using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public bool LockedRotation { get; private set; }

    Quaternion _startRotation;
    Vector3 _localStartRotation;

    void Awake()
    {
        _startRotation = transform.rotation;
        _localStartRotation = transform.localEulerAngles;
    }

    void LateUpdate()
    {
        if(!LockedRotation) { return; }

        transform.rotation = _startRotation;
    }

    public void ToggleLockState() // TODO Options Menu to use ToggleLockState
    {
        LockedRotation = !LockedRotation;
        if(!LockedRotation)
        {
            transform.localEulerAngles = _localStartRotation;
        }
    }
}
