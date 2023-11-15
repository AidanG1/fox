using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator2D : MonoBehaviour
{
    public enum AnimationState
    {
        Idle,
        Walk,
        Jump
    }
    [Tooltip("Frames per second for animation")]
    public float animationFPS;
    [Tooltip("Animation frames for idle state")]
    public Sprite[] idleAnimation;
    [Tooltip("Animation frames for jump state")]
    public Sprite[] jumpAnimation;
    [Tooltip("Animation frames for walk state")]
    public Sprite[] walkAnimation;
    private AnimationState state = AnimationState.Idle;
    private Dictionary<AnimationState, Sprite[]> animationAtlas;
    private float frameTimer = 0;
    private int frameIndex = 0;
    private PlayerController controller;
    private Rigidbody2D rb2D;
    private SpriteRenderer sRenderer;

    void Start()
    {
        animationAtlas = new Dictionary<AnimationState, Sprite[]>();

        animationAtlas.Add(AnimationState.Idle, idleAnimation);
        animationAtlas.Add(AnimationState.Walk, walkAnimation);
        animationAtlas.Add(AnimationState.Jump, jumpAnimation);

        rb2D = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        AnimationState newState = GetAnimationState();
        if (state != newState)
        {
            TransitionToState(newState);
        }

        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0.0f)
        {
            Sprite[] anim = animationAtlas[state];

            frameTimer = 1 / animationFPS;
            frameIndex %= idleAnimation.Length;
            sRenderer.sprite = anim[frameIndex];

            frameIndex++;
        }
    }

    void TransitionToState(AnimationState newState)
    {
        frameTimer = 0.0f;
        frameIndex = 0;
        state = newState; 
    }

    AnimationState GetAnimationState()
    {
        if (controller.currentlyJumping)
        {
            return AnimationState.Jump;
        }
        if (Mathf.Abs(rb2D.velocity.x) > 0.1f)
        {
            return AnimationState.Walk;
        }
        return AnimationState.Idle;
    }
}
