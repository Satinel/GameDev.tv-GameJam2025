using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float _shakeMagnitude = 0.1f;
    [SerializeField] float _shakeDuration = 0.025f, _shakeDelay = 0.1f;

    Vector3 _startingCameraPosition;
    bool _isShaking;

    void OnEnable()
    {
        PlayerHealth.OnTakeDamage += PlayerHealth_OnTakeDamage;
    }

    void OnDisable()
    {
        PlayerHealth.OnTakeDamage -= PlayerHealth_OnTakeDamage;
    }

    void PlayerHealth_OnTakeDamage()
    {
        if(_isShaking) { return; }

        _isShaking = true;
        _startingCameraPosition = transform.position;

        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float timer = 0;
        while(timer < _shakeDuration)
        {
            timer += Time.deltaTime;
            transform.position = _startingCameraPosition + (Vector3)Random.insideUnitCircle * _shakeMagnitude;
            yield return new WaitForSeconds(_shakeDelay);
        }
        transform.position = _startingCameraPosition;
        _isShaking = false;
    }
}
