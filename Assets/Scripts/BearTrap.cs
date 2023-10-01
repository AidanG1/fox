using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    public Sprite[] trapSprites; // Array of trap sprites
    public float cycleSpeed = 0.5f; // Adjust the speed here
    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    private float timeSinceLastCycle = 0f;
    private bool activated = false;
    private bool waiting = false;
    private float waitTime = 2.0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = trapSprites[currentSpriteIndex];
    }

    private void Update()
    {
        if (activated)
        {
            if (!waiting)
            {
                timeSinceLastCycle += Time.deltaTime;
                if (timeSinceLastCycle >= cycleSpeed)
                {
                    // Cycle to the next sprite in the array
                    currentSpriteIndex = (currentSpriteIndex + 1) % trapSprites.Length;
                    spriteRenderer.sprite = trapSprites[currentSpriteIndex];

                    // Reset the timer
                    timeSinceLastCycle = 0f;
                    if (currentSpriteIndex == 0)
                    {
                        activated = false;
                    }

                    // Check if we need to wait
                    if (currentSpriteIndex == 2)
                    {
                        waiting = true;
                        StartCoroutine(WaitForAnimation());
                    }
                }
            }
        }
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(waitTime);

        // Resume animation after the delay
        waiting = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Berry"))
        {
            // Cycle to the next sprite immediately upon collision
            currentSpriteIndex = (currentSpriteIndex + 1) % trapSprites.Length;
            spriteRenderer.sprite = trapSprites[currentSpriteIndex];

            // Reset the timer
            timeSinceLastCycle = 0f;
            activated = true;
        }
    }
}