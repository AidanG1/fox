using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    public AudioClip clickSound; // Reference to the audio clip to play
    public SpriteRenderer buttonRenderer; // Reference to the SpriteRenderer for the button
    public Sprite pressedSprite; // The sprite to use when the button is pressed
    public Sprite buttonSprite;
    public ToggleCollision toggleCollision;
    public bool flipLeft = false;


    private void Start()
    {
        // Find and assign the ToggleCollision script reference
        toggleCollision = GetComponentInChildren<ToggleCollision>();

        UpdateButtonSprite();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Berry"))
        {
            // Destroy the Berry object
            Destroy(collision.gameObject);

            // Get the ToggleCollision component from the child object
            ToggleCollision toggleCollision = GetComponentInChildren<ToggleCollision>();

            if (toggleCollision != null)
            {
                // Toggle the collision state
                toggleCollision.Toggle();
            }

            // Play the assigned audio clip at the position of the GameObject
            if (clickSound != null)
            {
                AudioSource.PlayClipAtPoint(clickSound, transform.position);
                Debug.Log("click played");
            }

            // Toggle the button's sprite and press state
            if (toggleCollision.isPressed == true)
            {
                buttonRenderer.sprite = pressedSprite;
            }
            else
            {
                buttonRenderer.sprite = buttonSprite;
            }

        }
    }

    private void UpdateButtonSprite()
    {
        if (flipLeft)
        {
            buttonRenderer.flipX = true; // Flip the sprite about the Y-axis
        }
        else
        {
            buttonRenderer.flipX = false; // Reset the sprite's flip
        }
    }
}