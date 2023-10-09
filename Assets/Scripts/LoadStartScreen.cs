using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Public method to load a scene by name
    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
