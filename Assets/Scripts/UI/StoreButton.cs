using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StoreButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    public Trinket Trinket { get; private set; }

    [SerializeField] TextMeshProUGUI _buttonText, _toolTipTextArea;

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
        _toolTipTextArea.text = Trinket.ToolTipText;
    }

    public void SetTrinket(Trinket trinket)
    {
        Trinket = trinket;
        _buttonText.text = Trinket.StartingName;
    }
}
