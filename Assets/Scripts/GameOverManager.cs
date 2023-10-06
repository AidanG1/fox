using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;
    public TMP_Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        timerText.text = "Death Time: " + GameTimerManager.instance.GetTimerValue().ToString("F2");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}