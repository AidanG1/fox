using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



/// <summary>
/// In order to move left the player, a negative unitsToMove value must be set
/// initially. The move right 
///
/// 
/// </summary>
public class EnemyController : MonoBehaviour
{
    // negative to move left
    public int unitsToMove;
    public float moveSpeed;

    // space being travelled between
    private float startPos;
    private float endPos;

    // direction bools
    protected bool isFacingRight;
    protected bool moveRight;
    protected bool moveUp;


    protected Rigidbody2D enemyRigidBody2D;

    // Start is called before the first frame update
    // Start or Awake
    protected virtual void Start()
    {
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        isFacingRight = transform.localScale.x > 0;

        // Calculate startPos and endPos based on initial position and unitsToMove
        startPos = Mathf.Min(transform.position.x, transform.position.x + unitsToMove);
        endPos = Mathf.Max(transform.position.x, transform.position.x + unitsToMove);

    }


    protected void movementLeftRight()
    {
        // current position
        float currPosX = transform.position.x;

        // moving right
        if (moveRight)
        {
            enemyRigidBody2D.velocity = new Vector2(moveSpeed, enemyRigidBody2D.velocity.y);

            // Check if enemy needs to change direction
            if (currPosX >= endPos)
            {
                moveRight = false;
                Flip();
            }
        }
        else // moving left
        {
            enemyRigidBody2D.velocity = new Vector2(-moveSpeed, enemyRigidBody2D.velocity.y);

            // Check if enemy needs to change direction
            if (currPosX <= startPos)
            {
                moveRight = true;
                Flip();
            }
        }
    }

    protected void movementUpDown()
    {
        // Current position
        float currPosY = transform.position.y;

        // Moving up
        if (moveUp)
        {
            enemyRigidBody2D.velocity = new Vector2(enemyRigidBody2D.velocity.x, moveSpeed);

            // Check if enemy needs to change direction
            if (currPosY >= endPos)
            {
                moveUp = false;
                //Flip();
            }
        }
        else // Moving down
        {
            enemyRigidBody2D.velocity = new Vector2(enemyRigidBody2D.velocity.x, -moveSpeed);

            // Check if enemy needs to change direction
            if (currPosY <= startPos)
            {
                moveUp = true;
                //Flip();
            }
        }
    }



    // Figure out if this is necessary 
    protected void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x,
            transform.localScale.y, transform.localScale.z);

        isFacingRight = transform.localScale.x > 0;
    }

    /// <summary>
    /// Function destroys a rat if hit by a bullet or restarts scene if a
    /// collision between rat and fox.
    /// </summary>
    /// 
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            //// reset scene if rat hit by bullet
            //Debug.Log("Player Killed");
            //Debug.Log("Reset Player Health");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(40);

        } else if (!collision.gameObject.CompareTag("Knife")){
            //destroy self(rat) if hit by bullet
            Destroy(gameObject);
        }
    }

}
