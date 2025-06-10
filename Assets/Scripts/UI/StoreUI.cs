using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    public static event Action OnExitStore;

    [SerializeField] List<Trinket> _saleItems;
    [SerializeField] StoreButton[] _storeButtons;
    [SerializeField] List<Button> _buttonList = new();
    [SerializeField] TextMeshProUGUI _priceText, _descriptionText;
    [SerializeField] int _itemPrice, _priceIncrease;
    [SerializeField] bool _isTutorial;
    [SerializeField] GameObject _storeWindow, _tooPoorWindow, _sorryButton, _leaveButton;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _purchaseClip;

    PlayerStats _playerStats;
    PlayerInventory _playerInventory;

    void Start()
    {
        Store.OnEnteredStore += Store_OnEnteredStore;
        OptionsMenu.OnOptionsClosed += OptionsMenu_OnOptionsClosed;
        RestAreaUI.OnRestAreaUsed += RestAreaUI_OnRestAreaUsed;

        SetupItems();
    }

    void OnDestroy()
    {
        Store.OnEnteredStore -= Store_OnEnteredStore;
        OptionsMenu.OnOptionsClosed -= OptionsMenu_OnOptionsClosed;
        RestAreaUI.OnRestAreaUsed -= RestAreaUI_OnRestAreaUsed;
    }

    void SetupItems()
    {
        if(_saleItems.Count > 3)
        {
            _saleItems.Shuffle();
        }

        for(int i = 0; i < 3; i++)
        {
            _storeButtons[i].SetTrinket(_saleItems[i]);
            _storeButtons[i].gameObject.SetActive(true);
        }

        _priceText.text = $"Price: {_itemPrice.FormatLargeNumbers()} BugBucks";

        if(!_playerStats)
        {
            _playerStats = FindFirstObjectByType<PlayerStats>();
        }
        if(!_playerInventory)
        {
            _playerInventory = _playerStats.GetComponent<PlayerInventory>();
        }
        SetButtonNavigations();
    }

    void Store_OnEnteredStore(Transform t)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_leaveButton);
        _descriptionText.text = string.Empty;
    }

    void OptionsMenu_OnOptionsClosed()
    {
        if(!_storeWindow.activeSelf) { return; }

        if(_tooPoorWindow.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_sorryButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_leaveButton);
        }
    }

    void RestAreaUI_OnRestAreaUsed()
    {
        if(_isTutorial) { return; }

        _itemPrice += _priceIncrease;
        SetupItems();
    }

    public void LeaveStoreButton() // UI Button
    {
        OnExitStore?.Invoke();
        _tooPoorWindow.SetActive(false);
    }

    public void ClosePoorWindowButton() // UI Button
    {
        _tooPoorWindow.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_leaveButton);
    }

    public void AttemptPurchase(int index) // UI Buttons
    {
        if(_playerStats.Money < _itemPrice)
        {
            _tooPoorWindow.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_sorryButton);
        }
        else
        {
            _playerStats.ChangeMoney(-_itemPrice);
            _playerInventory.AddTrinket(_saleItems[index]);
            _audioSource.PlayOneShot(_purchaseClip);
            _storeButtons[index].SellTrinket();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_leaveButton);
            SetButtonNavigations();
            _descriptionText.text = string.Empty;
        }
    }

    void SetButtonNavigations() //  What a nonsense method this is
    {
        for(int i = 0; i < _buttonList.Count; i++)
        {
            if(!_buttonList[i].interactable) { continue; }

            Navigation navigation = _buttonList[i].navigation;
            navigation.mode = Navigation.Mode.Explicit;
            if(i < _buttonList.Count - 1)
            {
                if(_buttonList[i + 1].interactable)
                {
                    navigation.selectOnRight = _buttonList[i + 1];
                }
                else if(i < _buttonList.Count - 2 && _buttonList[i + 2].interactable)
                {
                    navigation.selectOnRight = _buttonList[i + 2];
                }
                else
                {
                    navigation.selectOnRight = _buttonList[3];
                }
            }
            if(i > 0)
            {
                if(_buttonList[i - 1].interactable)
                {
                    navigation.selectOnLeft = _buttonList[i - 1];
                }
                else if(i > 1 && _buttonList[i - 2].interactable)
                {
                    navigation.selectOnLeft = _buttonList[i - 2];
                }
                else
                {
                    navigation.selectOnLeft = _buttonList[3];
                }
            }
            if(i != _buttonList.Count)
            {
                navigation.selectOnUp = _buttonList[3];
                navigation.selectOnDown = _buttonList[3];
            }
            else
            {
                if(_buttonList[0].interactable)
                {
                    navigation.selectOnUp = _buttonList[0];
                }
                else if(_buttonList[1].interactable)
                {
                    navigation.selectOnUp = _buttonList[1];
                }
                else if(_buttonList[2].interactable)
                {
                    navigation.selectOnUp = _buttonList[2];
                }
            }
            _buttonList[i].navigation = navigation;
        }
    }

    public void SetFirstInteractable()
    {
        foreach(Button button in _buttonList)
        {
            if(button.interactable)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(button.gameObject);
                break;
            }
        }
    }
}
