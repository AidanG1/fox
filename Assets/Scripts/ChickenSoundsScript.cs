using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSoundScript : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public GameObject fox; // Reference to the fox GameObject
    public float maxVolume = 1.0f; // Maximum volume when the fox is at the closest distance
    public float minVolume = 0.1f; // Minimum volume when the fox is at the farthest distance
    public float maxDistance = 10.0f; // Maximum distance at which audio can be heard
    private bool hasCollided = false;

    private void Update()
    {
        // Calculate the distance between this GameObject and the fox
        float distance = Vector3.Distance(transform.position, fox.transform.position);

        // Calculate the volume based on distance
        float volume = Mathf.Clamp01(1 - (distance / maxDistance));
        volume = Mathf.Lerp(minVolume, maxVolume, volume);

        // Set the volume of the AudioSource
        audioSource.volume = volume;

        // Play the AudioClip on loop
        if (!hasCollided)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Stop();
            hasCollided = true;
        }
    }
}