using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [Tooltip("The speed at which the enemy moves.")]
    public float moveSpeed;
    [Tooltip("The number of units the enemy will move before turning around.")]
    public int unitsToMove; // use negative units to move left

    // space being traveled between
    private float endPos;
    private float startPos;

    // direction bools
    protected bool isFacingRight;
    protected bool moveRight;
    protected bool moveUp;
    protected Rigidbody2D enemyRigidBody2D;

    protected void GetBounds(bool horizontal)
    {
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        isFacingRight = transform.localScale.x > 0;

        if (horizontal)
        {
            startPos = Mathf.Min(transform.position.x, transform.position.x + unitsToMove);
            endPos = Mathf.Max(transform.position.x, transform.position.x + unitsToMove);
        } else
        {
            startPos = Mathf.Min(transform.position.y, transform.position.y + unitsToMove);
            endPos = Mathf.Max(transform.position.y, transform.position.y + unitsToMove);
        }
    }

    protected void MovementLeftRight()
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

                // Only flipping in this method because the base rat moves
                // between two set horizontal points.
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

                // Only flipping in this method because the base rat moves
                // between set points horizontal points.

                Flip();
            }
        }
    }

    protected void MovementUpDown()
    {
        // Current position
        float currPosY = transform.position.y;

        // Moving up
        if (moveUp)
        {
            enemyRigidBody2D.velocity = new
                Vector2(enemyRigidBody2D.velocity.x, moveSpeed);

            // Check if enemy needs to change direction
            if (currPosY >= endPos)
            {
                moveUp = false;                
            }
        }
        else // Moving down
        {
            enemyRigidBody2D.velocity = new
                Vector2(enemyRigidBody2D.velocity.x, -moveSpeed);

            // Check if enemy needs to change direction
            if (currPosY <= startPos)
            {
                moveUp = true;
            }
        }
    }

    protected void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x,
            transform.localScale.y, transform.localScale.z);

        isFacingRight = transform.localScale.x > 0;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // reset scene if rat hit by bullet
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(40);

        } else if (!collision.gameObject.CompareTag("Knife")){
            //destroy self(rat) if hit by bullet
            Destroy(gameObject);
        }
    }

}
