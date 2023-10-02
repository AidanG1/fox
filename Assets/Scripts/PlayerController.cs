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
    public float health = 100f;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float timeUntilRun = 0.5f;
    public float gravityScale = 10f;
    public float jumpForce = 20f;
    public float maxJumpForce = 28f;
    public float maxJumpTime = 0.5f;
    public float jumpBoostBuffer = 0.1f;
    public bool currentlyJumping = false;
    private float timeJumping = 0f;

    private float timeAtMaxHorizontalSpeed = 0f;

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
        currHealth = health;
        healthBar.MaxHealth(health);
        
    }

    // Update is called once per frame
    void Update()
    {
        ManageGround();
        ManageInputs();
        if (!isImmobile)
        {
            ManageJumping();
            ManageWalking();
        }
        ManageShooting();
        cantMove();
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
    }

    void ManageWalking()
    {
        if (Math.Abs(frameInput.horizontalInput) > 0.9)
        {
            timeAtMaxHorizontalSpeed += Time.deltaTime;
        }
        else
        {
            timeAtMaxHorizontalSpeed = 0f;
        }
        float speed = timeAtMaxHorizontalSpeed > timeUntilRun ? runSpeed : walkSpeed;

        rb.velocity = new Vector2(frameInput.horizontalInput * speed, rb.velocity.y);
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
    }

    // these two functions are so that the fox can't move when in the bear trap

    public void SetImmobile()
    {
        isImmobile = true;
        Debug.Log("immobile is set");
    }

    void cantMove()
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