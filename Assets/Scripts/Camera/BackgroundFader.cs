using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundFader : MonoBehaviour
{
    [Tooltip("Duration of the fade effect in seconds.")]
    public float fadeDuration = 1.0f;
    [Tooltip("The alpha value to fade to (and from).")]
    public float fadeThreshold = 0.07f;
    [Tooltip("Array of background sprites to fade between.")]
    public Sprite[] backgroundSprites;
    [Tooltip("Array of music clips per background")]
    [Header("Music")]
    // wrote this in in case we wanted different music for different parts of the map
    public AudioClip[] musicClips;
    [Tooltip("Reference to the SpriteRenderer component displaying the background sprite.")]
    public float musicVolume = 0.3f;
    public SpriteRenderer backgroundRenderer;
    private AudioSource audioSource;
    private int currentIndex = 0;


    private void Start()
    {
        // Display the initial background sprite
        backgroundRenderer.sprite = backgroundSprites[currentIndex];

        audioSource = GetComponent<AudioSource>();

        // Play the music
        if (musicClips != null && musicClips.Length > currentIndex && musicClips[currentIndex] != null)
        {
            audioSource.clip = musicClips[currentIndex];

            // 30% volume
            audioSource.volume = musicVolume;

            // loop the music
            audioSource.loop = true;

            audioSource.Play();
        }
    }

    // Call this method to initiate a fade transition to the next background sprite
    public void TransitionToNextBackground(int spriteIndex)
    {

        if (currentIndex != spriteIndex)
        {
            currentIndex = spriteIndex;

            // Play the music
            if (musicClips != null && musicClips.Length > currentIndex && musicClips[currentIndex] != null)
            {
                audioSource.clip = musicClips[spriteIndex];
                audioSource.Play();
            }

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