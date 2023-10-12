using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;

    void Start()
    {
        // make sure the pause panel is hidden
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        // check for clicks of the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if the pause panel is active, resume the game
            if (PausePanel.activeSelf)
            {
                ResumeGame();
            }
            // otherwise, pause the game
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        // reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
