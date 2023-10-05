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
    private float waitTime = 3.0f;

    // audio clips for the bear trap
    public AudioClip[] trapSounds; // An array of audio clips

    // berry and player activated is different
    private bool playerAct = false;
    private bool berryAct = false;

    // Reference to the original collider
    private Collider2D originalCollider;

    // Reference to the new collider
    private Collider2D newCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = trapSprites[currentSpriteIndex];

        originalCollider = GetComponent<Collider2D>();
        newCollider = transform.GetChild(0).GetComponent<Collider2D>(); // Assuming the new collider is the first child
        newCollider.enabled = false; // Disable the new collider initially
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

                        if (berryAct)
                        {
                            originalCollider.enabled = false;
                            newCollider.enabled = true;
                        }
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

        if (berryAct)
        {
            originalCollider.enabled = true;
            newCollider.enabled = false;
        }
        playerAct = false;

        berryAct = false;

        AudioSource.PlayClipAtPoint(trapSounds[1], transform.position);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Berry"))
        {
            AudioSource.PlayClipAtPoint(trapSounds[0], transform.position);

            // Cycle to the next sprite immediately upon collision
            currentSpriteIndex = (currentSpriteIndex + 1) % trapSprites.Length;
            spriteRenderer.sprite = trapSprites[currentSpriteIndex];

            // Reset the timer
            timeSinceLastCycle = 0f;
            activated = true;
            if (collision.gameObject.CompareTag("Player")){
                playerAct = true;

                collision.gameObject.GetComponent<PlayerController>().SetImmobile();

            }
            if (collision.gameObject.CompareTag("Berry")){
                berryAct = true;
            }
        }
    }
}