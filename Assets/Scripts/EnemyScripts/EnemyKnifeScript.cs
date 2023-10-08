using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        rb2d = GetComponent<Rigidbody2D>();
        // maybe replace this with GameObject script type ex in Rat enterCollider
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb2d.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            
        }
        else if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("Trap") ||
            collision.gameObject.CompareTag("Border") ||
            collision.gameObject.CompareTag("MovingPlatform"))
        {
            Destroy(gameObject);
        }
    }
}
