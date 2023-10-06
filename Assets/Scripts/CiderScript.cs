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
            if (player.currentHealth < 100.0f)
            {
                AddHealth(addAmount);

                int randomIndex = Random.Range(0, collisionSounds.Length);

                // Get the randomly selected audio clip
                AudioClip randomClip = collisionSounds[randomIndex];

                // Create a temporary GameObject to play the audio
                GameObject audioSourceObject = new GameObject("TemporaryAudioSource");
                AudioSource tempAudioSource = audioSourceObject.AddComponent<AudioSource>();

                // Set the volume of the temporary audio source to make it louder
                float louderVolume = 2.0f; // Adjust this value to make it louder or quieter
                tempAudioSource.volume = louderVolume;

                // Play the audio clip
                tempAudioSource.PlayOneShot(randomClip);

                // Destroy the temporary audio source object after the clip finishes playing
                Destroy(audioSourceObject, randomClip.length);
            }
        }
    }
}
