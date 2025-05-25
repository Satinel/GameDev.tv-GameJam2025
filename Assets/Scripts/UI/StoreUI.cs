using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using TMPro;

public class StoreUI : MonoBehaviour
{
    public static event Action OnExitStore;

    [SerializeField] List<Trinket> _saleItems;
    [SerializeField] StoreButton[] _storeButtons;
    [SerializeField] TextMeshProUGUI _priceText, _descriptionText;
    [SerializeField] int _itemPrice;
    [SerializeField] GameObject _tooPoorWindow, _sorryButton, _leaveButton;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _purchaseClip;

    PlayerStats _playerStats;
    PlayerInventory _playerInventory;

    void Start()
    {
        Store.OnEnteredStore += Store_OnEnteredStore;

        if(_saleItems.Count > 3)
        {
            _saleItems.Shuffle();
        }

        for(int i = 0; i < 3; i++)
        {
            _storeButtons[i].SetTrinket(_saleItems[i]);
        }

        _priceText.text = $"Price: {_itemPrice.FormatLargeNumbers()} BugBucks";

        _playerStats = FindFirstObjectByType<PlayerStats>();
        _playerInventory = _playerStats.GetComponent<PlayerInventory>();
    }

    void OnDestroy()
    {
        Store.OnEnteredStore -= Store_OnEnteredStore;
    }

    void Store_OnEnteredStore(Transform t)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_leaveButton);
        _descriptionText.text = string.Empty;
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
            EventSystem.current.SetSelectedGameObject(_tooPoorWindow);
        }
        else
        {
            _playerStats.ChangeMoney(-_itemPrice);
            _playerInventory.AddTrinket(_saleItems[index]);
            _audioSource.PlayOneShot(_purchaseClip);
            _storeButtons[index].gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_leaveButton);
            _descriptionText.text = string.Empty;
        }
    }
}
