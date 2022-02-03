using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingUIManager : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Arena");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
