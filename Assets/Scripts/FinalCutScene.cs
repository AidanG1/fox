using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;


public class FinalCutScene : MonoBehaviour
{
    [Header("Final Cut Scene Properties")]
    [Tooltip("The Canvas component")]
    public Canvas canvas; // Reference to the Canvas component
    [Tooltip("Gameplay components to disable when the cut scene starts")]
    public GameObject[] gameplayComponents;
    [Tooltip("GameTimerManager component")]
    public GameTimerManager gameTimerManager;
    [Tooltip("Reference to the text component for the game timer")]
    public TMP_Text gameTimerText;
    [Tooltip("Reference to the video player component")]
    public VideoPlayer videoPlayer;
    private float timerValue;

    private void Start()
    {
        // Disable the Canvas at the start (to hide it)
        canvas.enabled = false;

        videoPlayer.playOnAwake = false;

        // Prepare the video player without playing it
        videoPlayer.Prepare();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger is entered by an object tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Start playing the video
            Debug.Log("Collided with player");

            timerValue = gameTimerManager.GetTimerValue();
            HoldTimeData.instance.sharedValue = timerValue;

            gameTimerManager.EndGame();

            SceneManager.LoadScene("FinalCutScene");
        }
    }
}