using UnityEngine;

/// <summary>
/// This class updates the shooting rat position and calls a shooting method.
/// </summary>
public class ShootingRatController : EnemyController
{
    [Tooltip("The range at which the rat will shoot the player.")]
    public float shootingRange;
    [Tooltip("The knife to shoot at the player.")]
    public GameObject knife;
    [Tooltip("The position to shoot the knife from.")]
    public Transform knifePosition;

    private float timer;
    private GameObject player;

    public void Start()
    {
        // Setting horizontal bounds for shooting rat which walks between two
        // points
        // True because bounds are horizontal
        GetBounds(true);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Updating ShootingRat position
        MovementLeftRight();

        // Getting distance of player to shoot at
        float distance = Vector2.Distance(transform.position, player.transform.position);
        // Only shooting if distance is less than shooting range
        if (distance < shootingRange)
        {
            // Can only shoot if timer is greater than 2
            timer += Time.deltaTime;
            if (timer > 2)
            {
                // Reset and shoot at player
                timer = 0;
                Shoot(knife, knifePosition);
            }
        }
    }



}
