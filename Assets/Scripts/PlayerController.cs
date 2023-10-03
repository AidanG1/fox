using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Basic player variables
    private Rigidbody2D rb;
    private PolygonCollider2D pc;
    private SpriteRenderer sr;
    private FrameInput frameInput = new FrameInput();
    private GameObject weaponParent;
    private GameObject weapon;

    // Player movement variables
    public float healthMax = 100f;
    public float walkSpeed = 5f;
    public float timeToWalk = 0.2f;
    public float runSpeed = 10f;
    public float timeToRun = 0.4f;
    public float timeUntilRun = 0.5f;
    private float runningTime = 0f;
    public float gravityScale = 10f;
    public float newJumpForce = 0.2f;
    public float jumpForce = 20f;
    public float maxJumpForce = 28f;
    public float maxJumpTime = 0.5f;
    public float jumpBoostBuffer = 0.1f;
    public bool currentlyJumping = false;
    private float timeJumping = 0f;

    // Player grounding variables
    private bool onGround = false;
    public Vector3 boxSize = new Vector3(0.5f, 0.3f, 1f);
    public float maxDistance = 0.1f;
    public LayerMask layerMask = 3; // ground

    // added in for implementing immobility when interacting with a bear trap
    private bool isImmobile = false;
    private float immobileTimer = 0f;
    public float waitTime = 3.3f; // Adjust the wait time as needed


    // variables for the health bar
    public HealthBarScript healthBar;
    public float currHealth;

    public float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.1f;
    private float jumpBufferTimeCounter;


    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component of the player
        rb = GetComponent<Rigidbody2D>();

        // Set the gravity scale of the player
        rb.gravityScale = gravityScale;

        // Get the PolygonCollider2D component of the player
        pc = GetComponent<PolygonCollider2D>();

        // Get the weapon of the player
        weaponParent = transform.GetChild(0).gameObject;
        weapon = weaponParent.transform.GetChild(0).gameObject;

        // Get the SpriteRenderer component of the player
        sr = GetComponent<SpriteRenderer>();

        // set max health at the start of the game
        currHealth = healthMax;
        healthBar.MaxHealth(healthMax);
    }

    // Update is called once per frame
    void Update()
    {
        ManageInputs();
        ManageGround();
        ManageJumpBuffer();
        ManageCoyote();
        ManageJumpBuffer();
        if (!isImmobile)
        {
            ManageWalking();
            ManageJumpingNew();
            // ManageJumping();
        }
        ManageShooting();
        ManageMovingPlatform();
        CantMove();
    }

    void ManageMovingPlatform()
    {
        // check if player is touching platform layer
        // boxcast down to see if moving platform is below
        // if so, move the player with the platform
        var bc = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, 7);
        if (bc)
        {
            // get the moving platform
            GameObject platform = bc.collider.gameObject;

            // move the player with the platform
            var vel = rb.velocity;
            vel.x += platform.GetComponent<MovingPlatform>().speed;
            rb.velocity = vel;
        }
    }

    void ManageJumpBuffer()
    {
        if (frameInput.jumpPressed)
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
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

    void ManageJumpingNew()
    {
        if (coyoteTimeCounter > 0 && jumpBufferTimeCounter > 0)
        {
            rb.velocity += new Vector2(0, newJumpForce);
            jumpBufferTimeCounter = 0;
        }

        if (frameInput.jumpUpPressed && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0;
        }
    }

    void ManageJumping()
    {
        // If on ground and jump is keyed down, add upwards force
        if (onGround && frameInput.jumpDownPressed)
        {
            currentlyJumping = true;
            rb.velocity += new Vector2(0, jumpForce);
        }

        // If in the air and jump is held down, add small upwards force (this should only apply up to maxJumpForce)
        if (!onGround && frameInput.jumpPressed && timeJumping > jumpBoostBuffer && timeJumping < maxJumpTime)
        {
            // this extra force should be applied over time (maxJumpTime - jumpBoostBuffer)
            float extraJumpForce = maxJumpForce - jumpForce;
            float extraJumpTime = maxJumpTime - jumpBoostBuffer;
            float extraJumpForcePerSecond = extraJumpForce / extraJumpTime;
            rb.velocity += new Vector2(0, extraJumpForcePerSecond * Time.deltaTime);
        }

        if (frameInput.jumpPressed)
        {
            timeJumping += Time.deltaTime;
        }
        else
        {
            timeJumping = 0f;
        }
    }

    void ManageInputs()
    {
        frameInput.horizontalInput = Input.GetAxis("Horizontal");
        frameInput.mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        frameInput.jumpDownPressed = Input.GetKeyDown(KeyCode.Space);
        frameInput.jumpPressed = Input.GetKey(KeyCode.Space);
        frameInput.jumpUpPressed = Input.GetKeyUp(KeyCode.Space);
        frameInput.shootPressed = Input.GetMouseButtonDown(0);
        frameInput.shiftPressed = Input.GetKey(KeyCode.LeftShift);
    }

    void ManageWalking()
    {
        // If the player is moving left or right, move the player
        if (frameInput.horizontalInput != 0)
        {
            if (frameInput.shiftPressed)
            {
                runningTime += Time.deltaTime;
                rb.velocity = new Vector2(frameInput.horizontalInput * Mathf.Lerp(walkSpeed, runSpeed, Math.Min(1, runningTime / timeToRun)), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(frameInput.horizontalInput * walkSpeed, rb.velocity.y);
            }
        }
        else
        {
            runningTime = 0f;
            rb.velocity = new Vector2(0, rb.velocity.y);
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
            //     weaponParent.transform.SetPositionAndRotation(_originalWeaponParentLocalPosition, Quaternion.Euler(0f, 0f, 0f));
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);

            // flip weapon parent to be in the same relative position
            // position needs to change too
            // weaponParent.transform.SetPositionAndRotation(new Vector3(_originalWeaponParentLocalPosition.x - 0.5f, _originalWeaponParentLocalPosition.y, _originalWeaponParentLocalPosition.z), Quaternion.Euler(0f, 180f, 0f)); 
        }

        // on left click, shoot the weapon
        if (Input.GetMouseButtonDown(0))
        {
            weapon.GetComponent<Weapon>().Shoot();
        }

    }
    void ManageGround()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        healthBar.SetHealth(currHealth);
        Debug.Log(currHealth);
        if (currHealth <= 0)
        {
            // restart the level
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        StartCoroutine(BlinkColor(1, Color.red));
    }

    public void AddHealth(float addedHealth)
    {
        currHealth = Mathf.Min(currHealth + addedHealth, 100.0f);
        healthBar.SetHealth(currHealth);

        StartCoroutine(BlinkColor(1, Color.green));
    }

    // these two functions are so that the fox can't move when in the bear trap

    public void SetImmobile()
    {
        isImmobile = true;
        Debug.Log("immobile is set");
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
    public bool shiftPressed;
}