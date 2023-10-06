using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AudioClip bulletSound;
    public int maxRicochets = 0;
    private int ricochets = 0;
    public int maxPierces = 0;
    private int pierces = 0;

    public float velocity = 15f;

    public bool shouldExplode = false;
    public GameObject explosionPrefab;
    public float explosionRadius = 3f;
    public float explosionForce = 10f;
    public float explosionDuration = 0.5f;
    public float fireRate = 1f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.forward * velocity;

        // make muzzle flash by making the bullet white and big on frame 1
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
        // transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        // play bullet sound
        if (bulletSound != null)
        {
            AudioSource.PlayClipAtPoint(bulletSound, transform.position);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (shouldExplode && !collision.gameObject.CompareTag("player"))
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
                    // print(collider);
                    // Vector2 direction = (rb.transform.position - transform.position).normalized;
                    // rb.velocity += direction * explosionForce;
                    if 
                }
            }

            // Destroy bullet and explosion after delay
            Destroy(explosion, explosionDuration);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Border") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            if (ricochets < maxRicochets)
            {
                // ricochet
                ricochets++;
            }
            else
            {
                // destroy
                Destroy(gameObject);
            }
        }
    }
}
