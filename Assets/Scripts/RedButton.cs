using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    [Header("Button Settings")]
    [Tooltip("The audio clip to play when the button is pressed")]
    public AudioClip clickSound; // Reference to the audio clip to play
    [Tooltip("Whether the button should be flipped left")]
    public bool flipLeft = false;
    [Tooltip("The sprite to use when the button is not pressed")]
    public Sprite buttonSprite;
    [Tooltip("The sprite to use when the button is pressed")]
    public Sprite pressedSprite;
    [Tooltip("The SpriteRenderer for the button")]
    public SpriteRenderer buttonRenderer;
    [Tooltip("The ToggleCollision script reference")]
    public ToggleCollision toggleCollision;

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
            collision.gameObject.GetComponent<Bullet>().CallExplode();
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