using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Movement")]
    [Tooltip("The speed at which the camera moves to the next position.")]
    public float cameraMoveSpeed = 5.0f;
    [Tooltip("The amount to offset the camera's y position.")]
    public float yOffset = 3.0f;
    [Tooltip("The GameObject that contains the BackgroundFader script.")]
    public GameObject backgroundManager;
    [Tooltip("The index of the background sprite to transition to.")]
    public int spriteIndex;
    private BackgroundFader backgroundFader;
    private bool isCameraMoving = false;
    private static CameraMovement lastCameraMovement;
    private Vector3 targetPosition;

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

            if (Vector3.Distance(Camera.main.transform.position, targetPosition) < 0.1f)
            {
                Camera.main.transform.position = targetPosition;
                isCameraMoving = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCameraMoving)
        {
            // Check if the current CameraMovement is the same as the last one
            if (lastCameraMovement != this)
            {
                // Reset the previous camera movement if it was still executing
                if (lastCameraMovement != null && lastCameraMovement.isCameraMoving)
                {
                    lastCameraMovement.ResetCameraMovement();
                }

                // Get the position of the GameObject the FOX collided with
                targetPosition = new Vector3(
                    transform.position.x,
                    transform.position.y + yOffset,
                    Camera.main.transform.position.z
                );

                GameTimerManager.instance.AddCameraMovementTime(targetPosition.y);

                // Set isCameraMoving to true to start the camera movement
                isCameraMoving = true;

                // Call the FadeToNewBackground method to initiate the background transition
                // Pass the new background sprite to the method
                
                backgroundFader.TransitionToNextBackground(spriteIndex);

                // Update the lastCameraMovement to the current one
                lastCameraMovement = this;
            }
        }
    }



    // Method to reset camera movement
    private void ResetCameraMovement()
    {
        isCameraMoving = false;
        // Reset any other camera-related variables as needed
    }
}