using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEnemy : MonoBehaviour
{
    public float animationFPS;
    public Sprite[] walkAnimation;


    private SpriteRenderer sRenderer;

    private float frameTimer = 0;
    private int frameIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
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

        //sRenderer.flipX = rb2D.velocity.x < 0.0f;

    }

  
}
