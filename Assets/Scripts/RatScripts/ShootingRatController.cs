using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRatController : EnemyController
{

    public GameObject knife;
    public Transform knifePosition;


    private float timer;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //movementLeftRight();

        float distance = Vector2.Distance(transform.position, player.transform.position);


        if (distance < 10)
        {
            timer += Time.deltaTime;
            if (timer > 2)
                    {
                        timer = 0;
                        shoot();
                    }
        }


    }


    void shoot()
    {
        Instantiate(knife, knifePosition.position, Quaternion.identity);
    }
     
}
