using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEnemy : MonoBehaviour
{
    [Tooltip("How many frames per second the animation should play at")]
    public float animationFPS;
    [Tooltip("The sprites to use for the animation")]
    public Sprite[] walkAnimation;
    private float frameTimer = 0;
    private int frameIndex = 0;
    private SpriteRenderer sRenderer;

    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0.0f)
        {
            frameTimer = 1 / animationFPS;
            frameIndex %= walkAnimation.Length;
            sRenderer.sprite = walkAnimation[frameIndex];
            frameIndex++;
        }
    }  
}
