using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimerManager : MonoBehaviour
{
    public static GameTimerManager instance;

    // GUI text for timer display
    public TMP_Text timerText;
    private float timer = 0f;
    private bool gameActive = true;
    private List<CameraMovementTime> cameraMovementTimes = new List<CameraMovementTime>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            // Update the timer
            timer += Time.deltaTime;
            // Display the timer in the GUI text
            timerText.text = timer.ToString("F2");
        }
    }

    void EndGame()
    {
        gameActive = false;

        // Save the game data
        string json = JsonUtility.ToJson(new GameData
        {
            gameplayTime = DateTime.Now,
            finalTime = timer,
            cameraMovementTimes = cameraMovementTimes
        });

        System.IO.File.WriteAllText(Application.persistentDataPath + "/gameData.json", json);
    }

    public void AddCameraMovementTime(float cameraY)
    {
        cameraMovementTimes.Add(new CameraMovementTime { cameraY = cameraY, time = timer });
    }
}


[System.Serializable]
public class GameData
{
    public DateTime gameplayTime;
    public float finalTime;
    public List<CameraMovementTime> cameraMovementTimes;
}

[System.Serializable]
public class CameraMovementTime
{
    public float cameraY;
    public float time;
}