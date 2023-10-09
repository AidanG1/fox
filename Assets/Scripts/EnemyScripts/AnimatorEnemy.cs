using UnityEngine;


/// <summary>
/// Script to animate enemies (Ratman and ShootingRatman) in the game when they
/// move. Frames can be cycled through quicker depending on FPS input.
///
/// Written by Manuel Santiago
/// </summary>
public class AnimatorEnemy : MonoBehaviour
{
    [Tooltip("How many frames per second the animation should play at")]
    public float animationFPS;
    [Tooltip("The sprites to use for the animation")]
    public Sprite[] walkAnimation;


    private float frameTimer = 0;

    // current index in array of sprites
    private int frameIndex = 0;
    private SpriteRenderer sRenderer;

    void Start()
    {
        //Assigning the sprite renderer from game object
        sRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Decrease the frameTimer by the time passed since the last frame
        frameTimer -= Time.deltaTime;

        // Check if it's time to update the animation frame.
        if (frameTimer <= 0.0f)
        {
            // Reset frameTimer to the time interval between frames
            frameTimer = 1 / animationFPS;
            // Ensure frameIndex stays within the bounds of the walkAnimation
            // array
            frameIndex %= walkAnimation.Length;
            // Set the sprite renderer's sprite to the current frame from the
            // walkAnimation array
            sRenderer.sprite = walkAnimation[frameIndex];
            // Increment the frameIndex for the next frame
            frameIndex++;
        }
    }  
}
