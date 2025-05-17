using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject _map;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    void ToggleMap()
    {
        _map.SetActive(!_map.activeSelf);
    }
}
