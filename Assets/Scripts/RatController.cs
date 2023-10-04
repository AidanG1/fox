using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : EnemyController
{
    // Update is called once per frame
    void Update()
    {
        movementLeftRight();

        // updating the facing direction
        if (moveRight && !isFacingRight)
        {
            Flip();
        }
        else if (!moveRight && isFacingRight)
        {
            Flip();
        }

        
    }

}
