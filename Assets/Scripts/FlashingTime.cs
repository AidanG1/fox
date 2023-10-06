using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlash : MonoBehaviour
{
    public TMP_Text flashingText;
    public float flashInterval = 0.5f; // Time interval for text flashing
    public float flashDuration = 24f; // Duration for flashing in seconds

    private bool isTextVisible = true;
    private float nextFlashTime;
    private float startTime;
    private bool flashingComplete = false;

    private void Start()
    {
        // Start flashing immediately
        nextFlashTime = Time.time + flashInterval;
        startTime = Time.time;
    }

    private void Update()
    {
        if (!flashingComplete)
        {
            // Check if it's time to toggle the text visibility
            if (Time.time >= nextFlashTime)
            {
                // Toggle the text visibility
                isTextVisible = !isTextVisible;

                // Update the TMP Text visibility
                flashingText.enabled = isTextVisible;

                // Set the time for the next flash
                nextFlashTime = Time.time + flashInterval;

                // Check if the flashing duration has exceeded the specified duration
                if (Time.time - startTime >= flashDuration)
                {
                    // Stop flashing by setting the flashingComplete flag to true
                    flashingComplete = true;
                }
            }
        }
    }
}