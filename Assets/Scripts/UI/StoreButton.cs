using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class StoreButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    public Trinket Trinket { get; private set; }
    [SerializeField] Button _button;

    [SerializeField] TextMeshProUGUI _buttonText, _toolTipTextArea;

    StoreUI _storeUI;

    void Awake()
    {
        _button = GetComponent<Button>();
        _storeUI = GetComponentInParent<StoreUI>();
        if(!_storeUI)
        {
            _storeUI = FindFirstObjectByType<StoreUI>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _toolTipTextArea.text = Trinket.ToolTipText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _toolTipTextArea.text = string.Empty;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(!_button.interactable)
        {
            _storeUI.SetFirstInteractable();
            return;
        }

        _toolTipTextArea.text = Trinket.ToolTipText;
    }

    public void SetTrinket(Trinket trinket)
    {
        Trinket = trinket;
        _buttonText.text = Trinket.StartingName;
        _button.interactable = true;
    }

    public void SellTrinket()
    {
        _buttonText.text = "SOLD OUT";
        _button.interactable = false;
    }
}
