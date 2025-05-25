using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject _inventoryWindow;
    [SerializeField] InventoryButton _invButtonPrefab;
    [SerializeField] TextMeshProUGUI _toolTipTextArea;
    [SerializeField] RectTransform _itemList;
    List<Button> _trinketButtons;

    void Start()
    {
        PlayerInventory.OnTrinketAdded += PlayerInventory_OnTrinketAdded;
        OptionsMenu.OnOptionsClosed += MenuOptions_OnOptionsClosed;
    }

    void OnDestroy()
    {
        PlayerInventory.OnTrinketAdded -= PlayerInventory_OnTrinketAdded;
        OptionsMenu.OnOptionsClosed -= MenuOptions_OnOptionsClosed;
    }

    void PlayerInventory_OnTrinketAdded(Trinket trinket)
    {
        InventoryButton inventoryButton = Instantiate(_invButtonPrefab, _itemList);
        inventoryButton.SetTrinket(trinket, _toolTipTextArea);
        Button button = inventoryButton.GetComponent<Button>();
        _trinketButtons.Insert(_trinketButtons.Count, button);
        if(_trinketButtons.Count > 1)
        {
            SetButtonNavigations();
        }
    }

    void MenuOptions_OnOptionsClosed()
    {
        if(_inventoryWindow.activeSelf)
        {
            if(_trinketButtons.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(_trinketButtons[0].gameObject);
            }
        }
    }

    void SetButtonNavigations()
    {
        for(int i = 0; i < _trinketButtons.Count - 1; i++)
        {
            Navigation navigation = _trinketButtons[i].navigation;
            navigation.mode = Navigation.Mode.Explicit;
            navigation.selectOnDown = _trinketButtons[i + 1];
            if(i > 0)
            {
                navigation.selectOnUp = _trinketButtons[i - 1];
            }
            _trinketButtons[i].navigation = navigation;
        }
    }

    public void Toggle()
    {
        if(_inventoryWindow.activeSelf)
        {
            Close();
        }
        else
        {
            _inventoryWindow.SetActive(true);
            if(_trinketButtons.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(_trinketButtons[0].gameObject);
            }
        }
    }

    public void Close()
    {
        _inventoryWindow.SetActive(false);
    }
}
