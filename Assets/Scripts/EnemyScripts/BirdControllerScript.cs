using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControllerScript : EnemyController
{
    [Tooltip("The range at which the bird will start shooting at the player.")]
    public float shootingRange;
    [Tooltip("The knife to shoot at the player.")]
    public GameObject knife;
    [Tooltip("The position to shoot the knife from.")]
    public Transform knifePosition;
    private float timer;
    private GameObject player;

    public void Start()
    {
        // executes parent class start method
        //base.Start();
        GetBounds(false);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        MovementUpDown();

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < shootingRange)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                timer = 0;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Instantiate(knife, knifePosition.position, Quaternion.identity);
    }
}
