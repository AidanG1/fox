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
    }

    // Update is called once per frame
    void Update()
    {
        ManageGround();
        ManageInputs();
        ManageJumping();
        ManageWalking();
        ManageShooting();
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
            print("Here!");
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
        // Flip the player
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void ManageShooting()
    {
        // point the weapon at the mouse
        Vector3 lookDirection = frameInput.mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

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
