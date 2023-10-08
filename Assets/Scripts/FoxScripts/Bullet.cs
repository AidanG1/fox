using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Properties")]
    [Tooltip("The sound to play when the bullet is fired.")]
    public AudioClip bulletSound;
    [Tooltip("How frequently the bullet can be fired.")]
    public float fireRate = 1f;
    [Tooltip("The speed of the bullet.")]
    public float velocity = 15f;
    [Tooltip("The maximum number of ricochets the bullet can make.")]
    public int maxRicochets = 0;
    private int ricochets = 0;

    [Header("Explosion Properties")]
    [Tooltip("Whether the bullet should explode on impact.")]
    public bool shouldExplode = false;
    [Tooltip("The duration of the explosion.")]
    public float explosionDuration = 0.5f;
    [Tooltip("The force of the explosion.")]
    public float explosionForce = 10f;
    [Tooltip("The radius of the explosion.")]
    public float explosionRadius = 3f;
    [Tooltip("The prefab of the explosion.")]
    public GameObject explosionPrefab;
    [Tooltip("The sound to play when the bullet ricochets.")]
    public AudioClip ricochetSound;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.forward * velocity;

        // play bullet sound
        if (bulletSound != null)
        {
            AudioSource.PlayClipAtPoint(bulletSound, transform.position);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (shouldExplode && !collision.gameObject.CompareTag("Player"))
        {
            print(collision.gameObject.tag);
            print(collision.gameObject.CompareTag("Player"));
            Explode();
        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Border") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            if (ricochets < maxRicochets)
            {
                ricochets++;

                // play ricochet sound
                if (ricochetSound != null)
                {
                    AudioSource.PlayClipAtPoint(ricochetSound, transform.position);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void Explode()
    {
        // explode
        // Instantiate explosion prefab
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // make explosion sized to explosion radius
        explosion.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);

        // Get all colliders within explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        // Apply explosion force to all rigidbodies within explosion radius
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    // print("player");
                    collider.gameObject.GetComponent<PlayerController>().TakeDamage(20);
                }
                else if (collider.gameObject.CompareTag("Enemy"))
                {
                    // destroy enemy
                    Destroy(collider.gameObject);
                }
                else if (collider.gameObject.CompareTag("Trap"))
                {
                    
                }
            }
        }

        // Destroy bullet and explosion after delay
        Destroy(explosion, explosionDuration);
        Destroy(gameObject);
    }

    public void CallExplode()
    {
        if (shouldExplode)
        {
            Explode();
        }
    }
}
