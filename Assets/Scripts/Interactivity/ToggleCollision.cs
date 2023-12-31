using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCollision : MonoBehaviour
{
    [Header("Collision Fade")]
    [Range(0, 1)] [Tooltip("The alpha value when collision is disabled")]
    public float fadeAlpha = 0.2f;
    [Tooltip("Whether the button is currently pressed")]
    public bool isPressed = false;
    [Tooltip("Whether the platform should start disabled")]
    public bool startDisabled = false;

    private Collider2D coll2D;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        // Get the Collider2D component
        coll2D = GetComponent<Collider2D>();

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Capture the original sprite color
        originalColor = spriteRenderer.color;

        // Set the initial state based on the public variable
        coll2D.enabled = !startDisabled;

        // Change the sprite's color if the initial state is disabled
        if (!coll2D.enabled)
        {
            Color fadedColor = originalColor;
            fadedColor.a = fadeAlpha;
            spriteRenderer.color = fadedColor;
        }
    }

    // Function to toggle the collision state
    public void Toggle()
    {
        coll2D.enabled = !coll2D.enabled;

        // Change the sprite's color when collision is enabled
        if (coll2D.enabled)
        {
            spriteRenderer.color = originalColor;
            isPressed = false;
        }
        else
        {
            // Change the sprite's color when collision is disabled to a faded version
            Color fadedColor = originalColor;
            fadedColor.a = fadeAlpha;
            spriteRenderer.color = fadedColor;
            isPressed = true;
        }

    }
}