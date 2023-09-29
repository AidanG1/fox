using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int unitsToMove;
    public float moveSpeed;

    // space being travelled between
    public float startPos;
    public float endPos;

    // direction bools
    public bool isFacingRight;
    public bool moveRight;


    protected Rigidbody2D enemyRigidBody2D;

    // Start is called before the first frame update
    // Start or Awake
    void Start()
    {
        enemyRigidBody2D = enemyRigidBody2D = GetComponent<Rigidbody2D>();
        startPos = transform.position.x;
        endPos = startPos + unitsToMove;

        // what does this do?
        // What x value is it refering to?
        // somehow reveleals if facing left or right
        isFacingRight = transform.localScale.x > 0;
    }

    protected void movementLeftRight()
    {
        // current position
        float currPosX = enemyRigidBody2D.position.x;

        // moving the player into direction being faced

        //moving right
        if (moveRight)
        {
            enemyRigidBody2D.velocity = new Vector2(moveSpeed,
                enemyRigidBody2D.velocity.y);

        }

        // checking if enemy has reached end bound
        if (currPosX >= endPos)
        {
            moveRight = false;
        }

        // moving left
        if (!moveRight)
        {
            enemyRigidBody2D.velocity = new Vector2(-moveSpeed,
                enemyRigidBody2D.velocity.y);

        }
        // checking if enemy has reached start bound
        if (currPosX <= startPos)
        {
            moveRight = true;
        }
    }

    // Figure out if this is necessary 
    protected void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x,
            transform.localScale.y, transform.localScale.z);
        isFacingRight = transform.localScale.x > 0;
    }
}
