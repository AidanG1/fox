using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject weaponUI;
    // max ricochets and max pierces are set in the bullet prefab    
    private GameObject stickTip;

    // Start is called before the first frame update
    void Start()
    {
        stickTip = transform.GetChild(0).gameObject;

        // set the active bullet prefab to the first bullet prefab in the list
        activeBulletPrefab = bulletPrefabs[0];
        UpdateUI();
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

    public void SetActiveBullet(int index)
    {
        activeBulletPrefab = bulletPrefabs[index];
    }

    public void AddBullet(GameObject bullet)
    {
        bulletPrefabs.Add(bullet);
        activeBulletPrefab = bulletPrefabs[^1];
        UpdateUI();
    }

    public void UpdateUI()
    {
        // destroy all children of weaponUI
        foreach (Transform child in weaponUI.transform)
        {
            Destroy(child.gameObject);
        }

        // each bullet should be size 20 pixels wide
        float width = 20f;
        float spacing = 5f;
        float position = 5f;
        float right = bulletPrefabs.Count * width + (bulletPrefabs.Count - 1) * spacing + position + 5f;
        float height = 30f;

        // set the width of weaponUI
        weaponUI.GetComponent<RectTransform>().sizeDelta = new Vector2(right, height);
        for (int i = 0; i < bulletPrefabs.Count; i++)
        {
            // update the UI
            GameObject bulletUI = new GameObject();
            Image bulletImage = bulletUI.AddComponent<Image>();
            bulletImage.sprite = bulletPrefabs[i].GetComponent<SpriteRenderer>().sprite;

            // set the parent of the bulletUI to the weaponUI
            bulletUI.transform.SetParent(weaponUI.transform);

            // set the position of the bulletUI
            bulletUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(position, 0);

            // set the size of the bulletUI
            bulletUI.GetComponent<RectTransform>().sizeDelta = new Vector2(width, width);

            if (bulletPrefabs[i] == activeBulletPrefab)
            {
                // set the color of the bulletUI
                bulletImage.color = Color.white;
            }
            else
            {
                // set the color of the bulletUI
                bulletImage.color = Color.gray;
            }

            // set the position for the next bulletUI
            position += width + spacing;
        }
    }
}
