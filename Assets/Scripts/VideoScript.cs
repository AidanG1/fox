using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    public Canvas canvas; // Reference to the Canvas component

    public VideoPlayer videoPlayer;

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
            Debug.Log("collided with player");
            videoPlayer.Play();

            // Enable the Canvas to display the RawImage and video
            canvas.enabled = true;

            // Optionally, you can pause other gameplay, switch scenes, or display UI here.
        }
    }
}