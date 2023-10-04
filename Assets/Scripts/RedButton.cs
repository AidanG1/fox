using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Berry"))
        {
            // Get the ToggleCollision component from the child object
            ToggleCollision toggleCollision = GetComponentInChildren<ToggleCollision>();

            if (toggleCollision != null)
            {
                // Toggle the collision state
                toggleCollision.Toggle();
            }
        }
    }
}
