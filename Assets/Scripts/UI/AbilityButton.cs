using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public static event Action<int> OnButtonFocused;
    public static event Action OnButtonNotFocused;

    [SerializeField] int _index;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnButtonFocused?.Invoke(_index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnButtonNotFocused?.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnButtonFocused?.Invoke(_index);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnButtonNotFocused?.Invoke();
    }
}
