using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraMoveSpeed = 5.0f; // Adjust this speed as needed
    private bool isCameraMoving = false;
    private Vector3 targetPosition;

    // Reference to the BackgroundManager GameObject
    public GameObject backgroundManager;
    public Sprite newBackgroundSprite; // Assign the new background sprite in the Inspector
    private int spriteIndex;

    // Reference to the BackgroundFader script on the backgroundManager GameObject
    private BackgroundFader backgroundFader;

    private void Start()
    {
        // Get the BackgroundFader script from the backgroundManager GameObject
        backgroundFader = backgroundManager.GetComponent<BackgroundFader>();
    }

    private void Update()
    {

        if (isCameraMoving)
        {
            // Interpolate the camera's position towards the target position over time
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                targetPosition,
                cameraMoveSpeed * Time.deltaTime
            );

            // Move the BackgroundManager GameObject to stay centered with the camera
            backgroundManager.transform.position = new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                backgroundManager.transform.position.z
            );

            // Check if the camera is close enough to the target position
            if (Vector3.Distance(Camera.main.transform.position, targetPosition) < 0.1f)
            {
                // Stop moving the camera once it's close enough
                Camera.main.transform.position = targetPosition;
                isCameraMoving = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCameraMoving) // Check if the FOX collided with the trigger and the camera isn't already moving
        {
            Debug.Log("FOX collided with the trigger");

            // Get the position of the GameObject the FOX collided with
            targetPosition = new Vector3(
                transform.position.x,
                transform.position.y,
                Camera.main.transform.position.z // Keep the same z-position (-10)
            );

            GameTimerManager.instance.AddCameraMovementTime(targetPosition.y);

            // Set isCameraMoving to true to start the camera movement
            isCameraMoving = true;

            // Call the FadeToNewBackground method to initiate the background transition
            // Pass the new background sprite to the method
            backgroundFader.TransitionToNextBackground(spriteIndex);
        }
    }
}