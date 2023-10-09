using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A parent class for all enemy controllers. In charge of updating positions,
/// shooting, and collisions between players and enemies.
/// 
/// Written by Manuel Santiago
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Tooltip("The speed at which the enemy moves.")]
    public float moveSpeed;
    [Tooltip("The number of units the enemy will move before turning around.")]
    public int unitsToMove; // use negative units to move left
    [Tooltip("The sound to play when the enemy dies.")]
    public AudioClip deathSound;

    // Space being traveled between
    private float endPos;
    private float startPos;

    // Direction bools used to determine how to update position during update call
    protected bool isFacingRight;
    protected bool moveRight;
    protected bool moveUp;
    protected Rigidbody2D enemyRigidBody2D;

    /// <summary>
    /// Sets bounds for enemy's to move between
    /// </summary>
    /// <param name="horizontal"></param>
    protected void GetBounds(bool horizontal)
    {
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        isFacingRight = transform.localScale.x > 0;

        // Math functions used to assure bounds don't break logic especially if
        // enemy moves left first.

        // If movement is horizontal
        if (horizontal)
        {
            startPos = Mathf.Min(transform.position.x,
                transform.position.x + unitsToMove);

            endPos = Mathf.Max(transform.position.x,
                transform.position.x + unitsToMove);
        } else // If movement is vertical
        {
            startPos = Mathf.Min(transform.position.y,
                transform.position.y + unitsToMove);

            endPos = Mathf.Max(transform.position.y,
                transform.position.y + unitsToMove);
        }
    }


    /// <summary>
    /// Method is called if movement is horizontal (Ratman and ShootingRatman)
    /// </summary>
    protected void MovementLeftRight()
    {
        // current position
        float currPosX = transform.position.x;

        // moving right
        if (moveRight)
        {
            // Change the movement of a game object with a Rigidbody2D. Change
            // horizontal speed while keeping vertical speed unchanged.
            enemyRigidBody2D.velocity = new Vector2(moveSpeed,
                enemyRigidBody2D.velocity.y);

            // Check if enemy needs to change direction
            if (currPosX >= endPos)
            {
                moveRight = false;

                // Only flipping in this method because rats move two set
                // horizontal points.
                Flip();
            }
        }
        else // moving left
        {
            enemyRigidBody2D.velocity = new Vector2(-moveSpeed,
                enemyRigidBody2D.velocity.y);

            // Check if enemy needs to change direction
            if (currPosX <= startPos)
            {
                moveRight = true;
                Flip();
            }
        }
    }

    /// <summary>
    /// Method is called if movement is vertical (Bird)
    /// </summary>
    protected void MovementUpDown()
    {
        // Current position
        float currPosY = transform.position.y;

        // Moving up
        if (moveUp)
        {
            // change the movement of a game object with a Rigidbody2D. Change
            // vertical speed while keeping horizontal speed unchanged.
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

    /// <summary>
    /// Flips sprite animation depending on direction of object and updates
    /// isFacingRight bool
    /// </summary>
    protected void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x,
            transform.localScale.y, transform.localScale.z);
        // checking if object still facing right
        // The local scale represents the object's size and orientation in its
        // local space.
        isFacingRight = transform.localScale.x > 0;
    }

    /// <summary>
    /// This method is called 
    /// Instantiate a 'knife' GameObject at the position defined by
    /// 'knifePosition' with no rotation (Quaternion.identity).
    /// </summary>
    /// <param name="knife"></param>
    /// <param name="knifePosition"></param>
    protected void Shoot(GameObject knife, Transform knifePosition)
    {
        // function used in Unity to create a new instance of a GameObject at a
        // specified position and rotation.
        Instantiate(knife, knifePosition.position, Quaternion.identity);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // reset scene if rat hit by bullet
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(40);

        } else if (!collision.gameObject.CompareTag("Knife")){
            // play death sound
            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            }

            //destroy self(rat) if hit by bullet
            Destroy(gameObject);
        }
    }

    

}
