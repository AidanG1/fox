using UnityEngine;

/// <summary>
/// The bird class is child class inherits from the EnemyController. This class
/// focuses on bird movement
/// </summary>
public class BirdControllerScript : EnemyController
{
    [Tooltip("The range at which the bird will start shooting at the player.")]
    public float shootingRange;
    [Tooltip("The knife to shoot at the player.")]
    public GameObject knife;
    [Tooltip("The position to shoot the knife from.")]
    public Transform knifePosition;
    private float timer;
    private GameObject player;

    public void Start()
    {
        // False because we want vertical bounds to be set and not horizonatal 
        GetBounds(false);

        // Find the player to shoot at
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Updating position of bird in vertical direction "flying"
        MovementUpDown();

        // Getting distance from player
        float distance = Vector2.Distance(transform.position, player.transform.position);

        // Can only shoot if player withing range
        if (distance < shootingRange)
        {
            timer += Time.deltaTime;
            // Can only shoot if two seconds have passed.
            if (timer > 2)
            {
                // Reset timer and shoot knife at player
                timer = 0;
                Shoot(knife, knifePosition);
            }
        }
    }

    
}
