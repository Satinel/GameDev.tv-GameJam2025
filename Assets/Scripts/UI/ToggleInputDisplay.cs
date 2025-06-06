using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleInputDisplay : MonoBehaviour
{
    [SerializeField] GameObject _keyboardDisplay, _gamepadDisplay;

    void Awake()
    {
        PlayerController.OnControlSchemeChanged += PlayerController_OnControlSchemeChanged;
    }

    void OnDestroy()
    {
        PlayerController.OnControlSchemeChanged -= PlayerController_OnControlSchemeChanged;
    }

    void PlayerController_OnControlSchemeChanged(PlayerInput input)
    {
        if(input.currentControlScheme == "Gamepad")
        {
            _keyboardDisplay.SetActive(false);
            _gamepadDisplay.SetActive(true);
        }
        else
        {
            _keyboardDisplay.SetActive(true);
            _gamepadDisplay.SetActive(false);
        }
    }
}
