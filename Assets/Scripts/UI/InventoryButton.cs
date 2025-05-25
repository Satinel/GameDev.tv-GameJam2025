using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Trinket Trinket { get; private set; }

    [SerializeField] TextMeshProUGUI _buttonText, _toolTipTextArea;

    void Start()
    {
        PlayerInventory.OnTrinketLevelled += PlayerInventory_OnTrinketLevelled;
    }

    void OnDestroy()
    {
        PlayerInventory.OnTrinketLevelled -= PlayerInventory_OnTrinketLevelled;
    }

    void PlayerInventory_OnTrinketLevelled(Trinket trinket)
    {
        if(trinket == Trinket)
        {
            _buttonText.text = Trinket.Name;
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
        _toolTipTextArea.text = Trinket.ToolTipText;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _toolTipTextArea.text = string.Empty;
    }

    public void SetTrinket(Trinket trinket, TextMeshProUGUI toolTip)
    {
        Trinket = trinket;
        _buttonText.text = Trinket.StartingName;
        _toolTipTextArea = toolTip;
    }

}
