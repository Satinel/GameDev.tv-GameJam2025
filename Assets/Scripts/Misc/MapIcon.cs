using UnityEngine;

public class MapIcon : MonoBehaviour
{
    MiniMapCamera _minMapCam;
    Transform _cameraTransform;

    void Awake()
    {
        _minMapCam = FindFirstObjectByType<MiniMapCamera>();
    }

    void Start()
    {
        _cameraTransform = _minMapCam.transform;
        transform.rotation = Quaternion.Euler(90, _cameraTransform.eulerAngles.y, 0);
    }

    void Update()
    {
        if(_minMapCam.LockedRotation) { return; }

        transform.rotation = Quaternion.Euler(90, _cameraTransform.eulerAngles.y, 0);
    }
}
