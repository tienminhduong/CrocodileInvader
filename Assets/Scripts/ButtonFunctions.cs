using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }
    public void BackHome()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
