using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    
    public float cameraMoveSpeed = 5.0f; // Adjust this speed as needed

    private bool isCameraMoving = false;
    private Vector3 targetPosition;

    void Update()
    {
        if (isCameraMoving)
        {
            // Interpolate the camera's position towards the target position over time
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                targetPosition,
                cameraMoveSpeed * Time.deltaTime
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("fox") && !isCameraMoving) // Check if the FOX collided with the trigger and the camera isn't already moving
        {
            Debug.Log("yes");
            // Get the position of the GameObject the fox collided with
            targetPosition = new Vector3(
                transform.position.x,
                transform.position.y,
                Camera.main.transform.position.z // Keep the same z-position (-10)
            );

            // Set isCameraMoving to true to start the camera movement
            isCameraMoving = true;
        }
    }
}
