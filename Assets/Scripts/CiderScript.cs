using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiderScript : MonoBehaviour
{
    public float addAmount = 10.0f;
    public PlayerController player;
    public HealthBarScript healthBar;

    public AudioClip[] collisionSounds; // An array of audio clips
    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            Debug.Log("Player assigned correctly: " + player.gameObject.name);
        }
        else
        {
            Debug.LogWarning("Player is not assigned in the Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddHealth(float amount)
    {
        // Modify the currHealth variable in the Player script
        player.AddHealth(amount);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.currHealth < 100.0f)
            {
                AddHealth(addAmount);

                int randomIndex = Random.Range(0, collisionSounds.Length);

                // Get the randomly selected audio clip
                AudioClip randomClip = collisionSounds[randomIndex];
                AudioSource.PlayClipAtPoint(randomClip, transform.position);
            }
        }
    }
}
