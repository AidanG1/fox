using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimerManager : MonoBehaviour
{
    [HideInInspector] // Hide in inspector, but still public
    public static GameTimerManager instance;
    [Header("Timer")]
    [Tooltip("The GUI text to display the timer.")]
    public TMP_Text timerText;
    [Header("Music")]
    [Tooltip("The music clip to play.")]
    public AudioClip musicClip;
    [Tooltip("The volume of the music.")]
    public float musicVolume = 0.3f;
    private bool gameActive = true;
    private float timer = 0f;
    private List<CameraMovementTime> cameraMovementTimes = new();

    void Start()
    {
        instance = this;
        timer = 0f;

        // Play the music
        if (musicClip != null)
        {
            AudioSource.PlayClipAtPoint(musicClip, Camera.main.transform.position);

            // 30% volume
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            audioSource.volume = musicVolume;

            // loop the music
            audioSource.loop = true;
        }
    }

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

    public void EndGame()
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

    public float GetTimerValue()
    {
        return timer;
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