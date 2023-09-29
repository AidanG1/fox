using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    [Tooltip("The sprite of the weapon")]
    public Sprite weaponSprite;
    [Tooltip("The prefab of the bullet")]
    public GameObject bulletPrefab;
    [Tooltip("The minimum time between shots")]
    public float fireRate = 0.5f;
    private float timeUntilNextShot = 0f;
    [Tooltip("The speed of the bullet")]
    public float bulletSpeed = 10f;
    [Tooltip("The number of bullets to shoot per shot")]
    public float quantity = 1f;
    [Tooltip("The angle between bullets")]
    public float spreadAngle = 0f;
    [Tooltip("The maximum number of times the bullet can ricochet off of walls")]
    public int maxRicochets = 0;
    [Tooltip("The maximum number of times the bullet can pierce enemies")]
    public int maxPierces = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = weaponSprite;
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilNextShot -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (timeUntilNextShot > 0)
        {
            // can't shoot
            return;
        }
        else
        {
            for (int i = 0; i < quantity; i++) // loop through quantity which is 
            {
                // generate a random angle between -spreadAngle and spreadAngle
                float angle = Random.Range(-spreadAngle, spreadAngle);

                // create a new bullet
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // set the bullet's rotation to the angle
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle + transform.rotation.eulerAngles.z);

                // set the bullet's velocity to bulletSpeed
                bullet.GetComponent<Bullet>().Shoot(bullet.transform.right * bulletSpeed);

                // set the bullet's maxRicochets to maxRicochets
                bullet.GetComponent<Bullet>().maxRicochets = maxRicochets;

                // set the bullet's maxPierces to maxPierces
                bullet.GetComponent<Bullet>().maxPierces = maxPierces;
            }
        }

        
    }
}
