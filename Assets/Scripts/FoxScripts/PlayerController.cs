using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Basic player variables
    private FrameInput frameInput = new FrameInput();
    private GameObject weapon;
    private GameObject weaponParent;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Player information variables
    [Header("Player Information")]
    [Tooltip("The maximum health of the player")]
    public float healthMax = 100f;
    [Tooltip("The walking speed of the player")]
    public float walkSpeed = 5f;
    [Tooltip("The jumping force of the player")]
    public float jumpForce = 0.72f;
    [Tooltip("The box dimensions for the ground check")]
    public Vector3 boxSize = new(0.5f, 0.5f, 1f);
    [Tooltip("The maximum distance for the ground check")]
    public float maxDistance = 0.9f;
    [Tooltip("The layer mask for the ground")]
    public LayerMask groundLayerMask = 3; // ground

    // Player grounding variables
    public bool currentlyJumping = false;
    private bool onGround = false;
    private float previousHorizontalInput = 0f;

    // added in for implementing immobility when interacting with a bear trap
    private bool isImmobile = false;
    private float immobileTimer = 0f;
    [Tooltip("The time the player is immobile when interacting with a bear trap")]
    public float waitTime = 3.3f; // Adjust the wait time as needed


    // variables for the health bar
    [Tooltip("The health bar of the player")]
    public HealthBarScript healthBar;
    [SerializeField] public float currentHealth;


    [Header("Fancy Jumping")]
    [Tooltip("The coyote time of the player")]
    public float coyoteTime = 0.1f;
    private float coyoteTimeCounter = 0.1f;

    [Tooltip("The jump buffer time of the player")]
    public float jumpBufferTime = 0.1f;
    private float jumpBufferTimeCounter = 0.1f;
    [Header("Sounds")]
    [Tooltip("The jump sound of the player")]
    public AudioClip jumpSound;
    private bool jumpSoundPlayed = false;
    [Tooltip("The slide sound of the player")]
    public AudioClip slideSound;
    [Tooltip("The hurt sound of the player")]
    public AudioClip hurtSound;

    void Start()
    {
        // Get the Rigidbody2D component of the player
        rb = GetComponent<Rigidbody2D>();

        // Get the weapon of the player
        weaponParent = transform.GetChild(0).gameObject;
        weapon = weaponParent.transform.GetChild(0).gameObject;

        // Get the SpriteRenderer component of the player
        sr = GetComponent<SpriteRenderer>();

        // set max health at the start of the game
        currentHealth = healthMax;
        healthBar.MaxHealth(healthMax);
    }

    void Update()
    {
        ManageInputs();
        ManageGround();
        ManageJumpBuffer();
        ManageCoyote();
        if (!isImmobile)
        {
            ManageWalking();
            ManageJumping();
        }
        ManageShooting();
        ManageMovingPlatform();
        CantMove();
    }
    void ManageInputs()
    {
        frameInput.horizontalInput = Input.GetAxis("Horizontal");
        frameInput.mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        frameInput.jumpDownPressed = Input.GetKeyDown(KeyCode.Space);
        frameInput.jumpPressed = Input.GetKey(KeyCode.Space);
        frameInput.jumpUpPressed = Input.GetKeyUp(KeyCode.Space);
        frameInput.shootPressed = Input.GetMouseButtonDown(0);
    }
    void ManageGround()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, groundLayerMask))
        {
            onGround = true;
            currentlyJumping = false;
        }
        else
        {
            onGround = false;
        }
    }
    void ManageJumpBuffer()
    {
        if (frameInput.jumpPressed)
        {
            jumpBufferTimeCounter = jumpBufferTime;
            currentlyJumping = true;
        }
        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
            currentlyJumping = false;
        }
    }
    void ManageCoyote()
    {
        if (onGround)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    void ManageMovingPlatform()
    {
        // check if player is touching platform layer
        // boxcast down to see if moving platform is below
        // if so, move the player with the platform
        var bc = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, 7);
        if (bc && bc.collider.CompareTag("MovingPlatform"))
        {
            // Get the MovingPlatform component from the collider's GameObject
            MovingPlatform platform = bc.collider.gameObject.GetComponent<MovingPlatform>();

            // Check if the platform component exists
            if (platform != null)
            {
                // Move the player with the platform
                var vel = rb.velocity;
                vel.x += platform.speed;
                rb.velocity = vel;
            }
        }
    }
    private float velocityXSmoothing = 0.0f;
    void ManageWalking()
    {
        // Calculate the desired horizontal velocity based on player input
        float targetVelocityX = frameInput.horizontalInput * walkSpeed;

        // smoothTIme influences how quick the fox runs after input. 
        float smoothTime = 0.01f; // Adjust this value for desired smoothing
        float velocityX = Mathf.SmoothDamp(rb.velocity.x, targetVelocityX, ref velocityXSmoothing, smoothTime);

        // Apply the new horizontal velocity
        rb.velocity = new Vector2(velocityX, rb.velocity.y);

        // Check for abrupt stops and play the slide sound
        if (Mathf.Abs(rb.velocity.x) > 2f && onGround && frameInput.horizontalInput == 0)
        {
            if (slideSound != null)
            {
                AudioSource.PlayClipAtPoint(slideSound, transform.position);
            }
        }
    }
    void ManageJumping()
    {
        // print("Coyote time: " + coyoteTimeCounter);
        // print("Jump buffer time: " + jumpBufferTimeCounter);
        if (coyoteTimeCounter > 0 && jumpBufferTimeCounter > 0)
        {
            rb.velocity += new Vector2(0, jumpForce * Time.deltaTime * 100);
            jumpBufferTimeCounter = 0;

            // Play the jump sound
            if (jumpSound != null && !jumpSoundPlayed)
            {
                AudioSource.PlayClipAtPoint(jumpSound, transform.position);
                jumpSoundPlayed = true;
            }
        }

        if (frameInput.jumpUpPressed && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0;
        }

        if (frameInput.jumpUpPressed)
        {
            jumpSoundPlayed = false;
        }
    }
    void ManageShooting()
    {
        // point the weapon at the mouse
        weaponParent.transform.LookAt(frameInput.mousePosition, Vector3.forward);

        // Flip the player based on mouse position
        if (frameInput.mousePosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // on left click, shoot the weapon
        if (Input.GetMouseButtonDown(0))
        {
            weapon.GetComponent<Weapon>().Shoot();
            // recoil is done by the weapon script
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }

    // these two functions are for the health bar
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("this is curr health");
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            // Game Over
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }

        // Play the hurt sound
        if (hurtSound != null)
        {
            AudioSource.PlayClipAtPoint(hurtSound, transform.position);
        }

        StartCoroutine(BlinkColor(2, Color.red));
    }
    public void AddHealth(float addedHealth)
    {
        currentHealth = Mathf.Min(currentHealth + addedHealth, 100.0f);
        healthBar.SetHealth(currentHealth);

        StartCoroutine(BlinkColor(2, Color.green));
    }

    // these two functions are so that the fox can't move when in the bear trap
    public void SetImmobile()
    {
        isImmobile = true;
        //Debug.Log("immobile is set");
    }

    void CantMove()
    {
        if (isImmobile)
        {
            // Update the timer
            immobileTimer += Time.deltaTime;

            // Check if the immobile duration is over
            if (immobileTimer >= waitTime)
            {
                // Reset the immobile state
                isImmobile = false;

                // Reset the timer
                immobileTimer = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            TakeDamage(20.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            weapon.GetComponent<Weapon>().AddBullet(collision.gameObject.GetComponent<Fruit>().bulletPrefab);

            // Play the fruit pickup sound
            if (collision.gameObject.GetComponent<Fruit>().fruitPickupSound != null)
            {
                AudioSource.PlayClipAtPoint(collision.gameObject.GetComponent<Fruit>().fruitPickupSound, transform.position);
            }

            // Destroy the fruit
            Destroy(collision.gameObject);
        }
    }

    IEnumerator BlinkColor(float time, Color color)
    {
        for (int i = 0; i < time / 0.2f; i++)
        {
            sr.color = color;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

public class FrameInput
{
    public float horizontalInput;
    public Vector3 mousePosition;
    public bool jumpDownPressed;
    public bool jumpPressed;
    public bool jumpUpPressed;
    public bool shootPressed;
}