using UnityEngine;

/// <summary>
/// Rat Controller moves basic rat left to right. Collision funcitino carried
/// out by parent class.
/// 
/// Written by Manuel Santiago
/// </summary>
public class RatController : EnemyController
{
    public void Start()
    {
        // Establishing running bounds for rat
        GetBounds(true);
    }

    void Update()
    {
        // Updating position of rat each frame
        // Moves between start and end bounds
        MovementLeftRight();
    }
}
