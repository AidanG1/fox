using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    [Tooltip("The prefab of the bullet")]
    public List<GameObject> bulletPrefabs;
    private GameObject activeBulletPrefab;
    [Tooltip("The minimum time between shots")]
    public float fireRate = 0.5f;
    private float timeUntilNextShot = 0f;
    [Tooltip("The number of bullets to shoot per shot")]
    public float quantity = 1f;
    [Tooltip("The angle between bullets")]
    public float maxSpread = 20f;

    // max ricochets and max pierces are set in the bullet prefab    
    private GameObject stickTip;

    // Start is called before the first frame update
    void Start()
    {
        stickTip = transform.GetChild(0).gameObject;

        // set the active bullet prefab to the first bullet prefab in the list
        activeBulletPrefab = bulletPrefabs[0];
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
                float angle = -maxSpread + 2 * maxSpread / quantity * i;

                // create a new bullet
                GameObject bullet = Instantiate(activeBulletPrefab, stickTip.transform.position, stickTip.transform.rotation * Quaternion.Euler(0, 0, angle));
            }
        }
    }

    public void SetBullets(List<GameObject> bullets)
    {
        bulletPrefabs = bullets;
        activeBulletPrefab = bulletPrefabs[0];
    }

    public void SetActiveBullet(int index)
    {
        activeBulletPrefab = bulletPrefabs[index];
    }

    public void AddBullet(GameObject bullet)
    {
        bulletPrefabs.Add(bullet);
        activeBulletPrefab = bulletPrefabs[^1];
    }
}
