using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
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

    private void Start()
    {
        int hs = 0;
        if (PlayerPrefs.HasKey("HighScore"))
            hs = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = hs.ToString();
    }
}
