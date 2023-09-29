using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : MonoBehaviour
{

    // maybe move five units?? not sure unites to move is
    public int unitsToMove;
    public float moveSpeed;
    Rigidbody2D enemyRigidBody2D;

    // positions for space between
    private float startPos;
    private float endPos;

    // direction bools
    public bool isFacingRight = true;
    public bool moveRight = true;



    void Awake()
    {
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        // Set the initial position as the starting position
        startPos = transform.position.x;
        endPos = startPos + unitsToMove;
        isFacingRight = transform.localScale.x > 0;
    }


    // Update is called once per frame
    void Update()
    {
        // updating the facing direction
        if (moveRight && !isFacingRight)
        {
            Flip();
        }
        else if (!moveRight && isFacingRight)
        {
            Flip();
        }


        // moving the player into facing direction

        //moving right
        if (moveRight)
        {
            enemyRigidBody2D.velocity = new Vector2(moveSpeed,
                enemyRigidBody2D.velocity.y);
            
        }

        if (enemyRigidBody2D.position.x >= endPos)
        {
            moveRight = false;
        }

        // moving left
        if (!moveRight)
        {
            enemyRigidBody2D.velocity = new Vector2(-moveSpeed,
                enemyRigidBody2D.velocity.y);

        }

        if (enemyRigidBody2D.position.x <= startPos)
        {
            moveRight = true;
        }

    }


    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        isFacingRight = transform.localScale.x > 0;
    }

}
