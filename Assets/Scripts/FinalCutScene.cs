using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class FinalCutScene : MonoBehaviour
{
    public Canvas canvas; // Reference to the Canvas component
    public VideoPlayer videoPlayer;
    public GameObject[] gameplayComponents;

    // Reference to the GameTimerManager script
    public GameTimerManager gameTimerManager;

    // Variable to store the timer value
    private float timerValue;

    //GameTimerText
    
    public TMP_Text gameTimerText;

    private void Start()
    {
        // Get the VideoPlayer component

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
            videoPlayer.Play();

            // Enable the Canvas to display the RawImage and video
            canvas.enabled = true;

            // Access the timer from GameTimerManager
            timerValue = gameTimerManager.GetTimerValue();
            gameTimerText.text = " " + timerValue.ToString("F2") + " sec";
            Debug.Log(timerValue);

            // Pause gameplay
            PauseGameplay();
        }
    }

    private void PauseGameplay()
    {
        // Iterate through the array of gameplay components and disable them
        foreach (GameObject component in gameplayComponents)
        {
            component.SetActive(false);
        }
    }

}