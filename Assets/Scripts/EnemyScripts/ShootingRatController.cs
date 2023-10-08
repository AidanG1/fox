using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRatController : EnemyController
{
    [Tooltip("The range at which the rat will shoot the player.")]
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
        GetBounds(true);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        MovementLeftRight();

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
