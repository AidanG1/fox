using UnityEngine;


/// <summary>
/// Script for the knife which will be used to hurt the player.
/// </summary>
public class EnemyKnifeScript : MonoBehaviour
{
    [Tooltip("Damage dealt to player")]
    public float damage;
    [Tooltip("Speed of knife")]
    public float force;
    private GameObject player;
    private Rigidbody2D rb2d;
    
    void Start()
    {
        // gets the Rigidbody2D component attached to the GameObject this script
        // is attached to
        rb2d = GetComponent<Rigidbody2D>();
        
        player = GameObject.FindGameObjectWithTag("Player");

        // This vector represents the direction from the current object to the player
        Vector3 direction = player.transform.position - transform.position;

        // Sets the velocity of the Rigidbody2D attached to the current object.
        // It uses the direction vector to determine the velocity components
        // along the X and Y axes. The normalized function ensures that the
        // velocity vector has a magnitude of 1 (unit vector), and then it's
        // scaled by the force value, which represents the speed or
        // force with which the object should move towards the player.
        rb2d.velocity = new Vector2(direction.x, direction.y).normalized * force;

        // Rotates the knife in the direction of the player
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    /// <summary>
    /// The knife is a trigger and hurts the player when they collide.
    /// The bullet always destroys itself when it collides with Ground, Trap,
    /// Border, MovingPlatform, Player.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            // Damages player
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            
        }
        // Bullet destroys itself after it collides with listed things below
        else if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("Trap") ||
            collision.gameObject.CompareTag("Border") ||
            collision.gameObject.CompareTag("MovingPlatform"))
        {
            Destroy(gameObject);
        }
    }
}
