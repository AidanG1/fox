using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Header("Fruit Properties")]
    [Tooltip("The bullet prefab to gain when the fruit is picked up.")]
    public GameObject bulletPrefab;
    [Tooltip("The sound to play when the fruit is picked up.")]
    public AudioClip fruitPickupSound;
}
