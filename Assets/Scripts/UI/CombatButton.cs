using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatButton : MonoBehaviour, ISelectHandler, IPointerClickHandler
{
    [SerializeField] Button _button;
    PlayerCombat _playerCombat;

    void Start()
    {
        _playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(!_button.interactable)
        {
            Invoke(nameof(Reselect), 0.1f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!_button.interactable)
        {
            Invoke(nameof(Reselect), 0.1f);
        }
    }

    void Reselect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _playerCombat.SelectFirstInteractableButton();
    }
}
