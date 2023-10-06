using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiderScript : MonoBehaviour
{
    public float addAmount = 10.0f;

    public AudioClip[] collisionSounds; // An array of audio clips

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().AddHealth(addAmount);

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
            Destroy(gameObject);
        }
    }
}
