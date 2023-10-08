using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlash : MonoBehaviour
{
    [Tooltip("The duration for which the text should flash in seconds.")]
    public float flashDuration = 24f;
    [Tooltip("The time interval for text flashing in seconds.")]
    public float flashInterval = 0.5f;
    [Tooltip("The TMP Text component to flash.")]
    public TMP_Text flashingText;

    private bool flashingComplete = false;
    private bool isTextVisible = true;
    private float nextFlashTime;
    private float startTime;

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