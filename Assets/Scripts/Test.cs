using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(2);
        }
    }
}
