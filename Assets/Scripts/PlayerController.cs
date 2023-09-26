using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PolygonCollider2D pc;
    public GameObject characterHolder;
    public float walkSpeed = 5f;
    public float gravityScale = 10f;

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
                StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
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

    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

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
