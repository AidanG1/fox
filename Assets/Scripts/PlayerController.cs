using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PolygonCollider2D pc;
    public float walkSpeed = 5f;
    public float gravityScale = 5f;

    private float horizontalInput;

    public float jumpForce = 10f;
    public float timeUntilAutoJump = 0.5f;
    private bool onGround = false;
    public float buttonTime = 0.3f;
    float jumpTime;
    bool jumping;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component of the player
        rb = GetComponent<Rigidbody2D>();

        // Set the gravity scale of the player
        rb.gravityScale = gravityScale;

        // Get the PolygonCollider2D component of the player
        pc = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onGround || jumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumping = true;
                jumpTime = 0;
            }
            if (jumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTime += Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.Space) | jumpTime > buttonTime)
            {
                jumping = false;
            }
        }
        // onGround = IsGrounded();
        // Get the horizontal input from the player
        horizontalInput = Input.GetAxis("Horizontal");

        // Move the player
        rb.velocity = new Vector2(horizontalInput * walkSpeed, rb.velocity.y);

        //print(rb.velocity.y);
        //print(onGround);
        //print(Input.GetKey(KeyCode.Space));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }
}
