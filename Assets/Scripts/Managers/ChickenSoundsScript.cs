using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSoundScript : MonoBehaviour
{
    [Tooltip("The AudioSource component")]
    public AudioSource audioSource;
    [Tooltip("The maximum distance at which audio can be heard")]
    public float maxDistance = 10.0f;
    [Tooltip("The maximum volume when the fox is at the closest distance")]
    public float maxVolume = 1.0f;
    [Tooltip("The minimum volume when the fox is at the farthest distance")]
    public float minVolume = 0.1f;
    [Tooltip("The fox GameObject")]
    public GameObject fox;
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