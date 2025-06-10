using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _selectedSFX, _clickedSFX;

    void Awake()
    {
        ButtonSounds.OnButtonSelected += ButtonSounds_OnButtonSelected;
        ButtonSounds.OnButtonClicked += ButtonSounds_OnButtonClicked;
    }

    void OnDestroy()
    {
        ButtonSounds.OnButtonSelected += ButtonSounds_OnButtonSelected;
        ButtonSounds.OnButtonClicked += ButtonSounds_OnButtonClicked;
    }

    void ButtonSounds_OnButtonSelected()
    {
        if(_audioSource && _selectedSFX)
        {
            _audioSource.PlayOneShot(_selectedSFX);
        }
    }

    void ButtonSounds_OnButtonClicked()
    {
        if(_audioSource && _clickedSFX)
        {
            _audioSource.PlayOneShot(_clickedSFX);
        }
    }
}
