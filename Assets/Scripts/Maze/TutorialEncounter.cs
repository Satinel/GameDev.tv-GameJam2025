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
        if(Random.Range(0, 3) > 0)
        {
            gameObject.SetActive(true);
        }
    }
}
