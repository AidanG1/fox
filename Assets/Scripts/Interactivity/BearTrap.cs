using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [Tooltip("The sound to play when the trap is activated.")]
    public AudioClip[] trapSounds; // An array of audio clips
    [Tooltip("The speed at which the trap cycles through its sprites.")]
    public float cycleSpeed = 0.5f; // Adjust the speed here
    [Tooltip("The sprites to cycle through when the trap is activated.")]
    public Sprite[] trapSprites; // Array of trap sprites
    private bool activated = false;
    private bool berryAct = false;
    private bool waiting = false;
    private Collider2D newCollider;
    private Collider2D originalCollider;
    private float timeSinceLastCycle = 0f;
    private float waitTime = 3.0f;
    private int currentSpriteIndex = 0;
    private SpriteRenderer spriteRenderer;

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
        
        berryAct = false;

        AudioSource.PlayClipAtPoint(trapSounds[1], transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Berry") || collision.gameObject.CompareTag("Explosion"))
        {
            AudioSource.PlayClipAtPoint(trapSounds[0], transform.position);

            // Cycle to the next sprite immediately upon collision
            currentSpriteIndex = (currentSpriteIndex + 1) % trapSprites.Length;
            spriteRenderer.sprite = trapSprites[currentSpriteIndex];

            // Reset the timer
            timeSinceLastCycle = 0f;
            activated = true;
            if (collision.gameObject.CompareTag("Player"))
            {
        
                collision.gameObject.GetComponent<PlayerController>().SetImmobile();
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(20);
            }
            if (collision.gameObject.CompareTag("Berry") || collision.gameObject.CompareTag("Explosion")) {
                berryAct = true;
            }
        }
    }
}