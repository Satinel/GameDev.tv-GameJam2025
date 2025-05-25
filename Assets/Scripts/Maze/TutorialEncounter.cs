using UnityEngine;

public class TutorialEncounter : MonoBehaviour
{
    void Start()
    {
        RestAreaUI.OnRestAreaUsed += RestAreaUI_OnRestAreaUsed;
    }

    void OnDestroy()
    {
        RestAreaUI.OnRestAreaUsed -= RestAreaUI_OnRestAreaUsed;
    }

    void RestAreaUI_OnRestAreaUsed()
    {
        gameObject.SetActive(true);
    }
}
