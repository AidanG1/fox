using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text gameText;

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
