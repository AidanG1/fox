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
    [Tooltip("The minimum time between shots, set using bullet")]
    private float fireRate = 0.5f;
    private float timeUntilNextShot = 0f;
    [Tooltip("The number of bullets to shoot per shot")]
    public float quantity = 1f;
    [Tooltip("The angle between bullets")]
    public float maxSpread = 20f;
    [Tooltip("The sound to play when the weapon is fired but fireRate does not work")]
    public AudioClip shootFailedSound;
    [Tooltip("The sound to play when the bullet is changed")]
    public AudioClip changeBulletSound;

    [Header("UI")]
    [Tooltip("The UI that displays the weapon bullet info")]
    public GameObject weaponUI;
    private GameObject stickTip;
    private GameObject muzzleFlash;
    private GameObject foxCharacter;

    // Start is called before the first frame update
    void Start()
    {
        stickTip = transform.GetChild(0).gameObject;
        muzzleFlash = transform.GetChild(1).gameObject;
        muzzleFlash.SetActive(false);

        // set the active bullet prefab to the first bullet prefab in the list
        SetActiveBullet(0, false);

        // set the fox character to the grandparent of the weapon
        // hierarchy: fox -> stick parent -> stick -> stick tip & muzzle flash
        // weapon is stick
        foxCharacter = transform.parent.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilNextShot -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveBullet(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveBullet(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetActiveBullet(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetActiveBullet(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetActiveBullet(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetActiveBullet(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetActiveBullet(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SetActiveBullet(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetActiveBullet(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetActiveBullet(9);
        }
    }

    public void Shoot()
    {
        if (timeUntilNextShot > 0)
        {
            // play shoot failed sound
            if (shootFailedSound != null)
            {
                AudioSource.PlayClipAtPoint(shootFailedSound, transform.position);
            }

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
                // add recoil
                Vector2 oppositeShoot = (foxCharacter.transform.position - bullet.transform.position);
                foxCharacter.GetComponent<Rigidbody2D>().velocity += oppositeShoot * GetRecoil();
                muzzleFlash.SetActive(true);
                timeUntilNextShot = fireRate;
                StartCoroutine(DisableMuzzleFlash());
            }
        }
    }

    IEnumerator DisableMuzzleFlash()
    {
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    public void SetActiveBullet(int index, bool playSound = true)
    {
        activeBulletPrefab = bulletPrefabs[index];
        fireRate = activeBulletPrefab.GetComponent<Bullet>().fireRate;
        UpdateUI();

        if (playSound && changeBulletSound != null)
        {
            AudioSource.PlayClipAtPoint(changeBulletSound, transform.position);
        }
    }

    public void AddBullet(GameObject bullet)
    {
        bulletPrefabs.Add(bullet);
        SetActiveBullet(bulletPrefabs.Count - 1);
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
        float position = 1f;
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
            bulletUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(position - right / 3, 2f);

            // set the size of the bulletUI
            bulletUI.GetComponent<RectTransform>().sizeDelta = new Vector2(height, height);

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

    public float GetRecoil()
    {
        return activeBulletPrefab.GetComponent<Rigidbody2D>().mass * activeBulletPrefab.GetComponent<Bullet>().velocity / 50f;
    }
}
