using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundFader : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer; // Reference to the SpriteRenderer component displaying the background sprite
    public float fadeDuration = 1.0f; // Duration of the fade effect

    public Sprite[] backgroundSprites; // Array of background sprites
    private int currentIndex = 0; // Index of the current background sprite
    public float fadeThreshold = 0.07f; // Adjust this value as needed

    private void Start()
    {
        // Display the initial background sprite
        backgroundRenderer.sprite = backgroundSprites[currentIndex];
    }

    // Call this method to initiate a fade transition to the next background sprite
    public void TransitionToNextBackground(int spriteIndex)
    {

        if (currentIndex != spriteIndex)
        {
            currentIndex = spriteIndex;
            Debug.Log(spriteIndex);
            // Start a fade-out coroutine
            StartCoroutine(CrossfadeToNext(backgroundSprites[currentIndex]));
        }
    }

    private IEnumerator CrossfadeToNext(Sprite newBackgroundSprite)
    {
        float fadeSpeed = 1.0f / fadeDuration;
        Color currentColor = backgroundRenderer.color;
        float currentAlpha = currentColor.a;

        // Define how much of the previous image remains when fading in the new image
        

        // Fade out the current background sprite while maintaining some visibility
        while (currentAlpha > fadeThreshold)
        {
            currentAlpha = Mathf.Max(fadeThreshold, currentAlpha - fadeSpeed * Time.deltaTime);
            backgroundRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentAlpha);
            yield return null;
        }

        // Switch to the new background sprite
        backgroundRenderer.sprite = newBackgroundSprite;

        // Fade in the new background sprite
        while (currentAlpha < 1f)
        {
            currentAlpha = Mathf.Min(1f, currentAlpha + fadeSpeed * Time.deltaTime);
            backgroundRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentAlpha);
            yield return null;
        }
    }
}