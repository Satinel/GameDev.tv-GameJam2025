using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    [SerializeField] TextMeshProUGUI _toolTipTextArea;
    [SerializeField] string _descriptionText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _toolTipTextArea.text = _descriptionText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _toolTipTextArea.text = "CHOOSE ONE";
    }

    public void OnSelect(BaseEventData eventData)
    {
        _toolTipTextArea.text = _descriptionText;
    }
}
