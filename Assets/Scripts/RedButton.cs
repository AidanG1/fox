using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    public AudioClip clickSound; // Reference to the audio clip to play

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Berry"))
        {
            // Get the ToggleCollision component from the child object
            ToggleCollision toggleCollision = GetComponentInChildren<ToggleCollision>();

            if (toggleCollision != null)
            {
                // Toggle the collision state
                toggleCollision.Toggle();
            }

            // Check if a SpriteRenderer and an audio clip are assigned
            if (clickSound != null)
            {
                // Play the assigned audio clip at the position of the GameObject
                AudioSource.PlayClipAtPoint(clickSound, transform.position);
                Debug.Log("click played");

                // You can also modify the sprite or color of the SpriteRenderer here if needed
                // For example, change the sprite color to indicate the button press:
                // spriteRenderer.color = Color.red;
            }
        }
    }
}