using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ButtonSounds : MonoBehaviour, ISelectHandler
{
    public static event Action OnButtonSelected;
    public static event Action OnButtonClicked;

    [SerializeField] Button _button;

    void Awake()
    {
        if(!_button)
        {
            _button = GetComponent<Button>();
        }
        if(_button)
        {
            _button.onClick.AddListener(ButtonOnClick);
        }
    }

    void OnDestroy()
    {
        if(!_button) { return; }

        _button.onClick.RemoveListener(ButtonOnClick);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(_button.interactable)
        {
            OnButtonSelected?.Invoke();
        }
    }

    void ButtonOnClick()
    {
        OnButtonClicked?.Invoke();
    }
}
