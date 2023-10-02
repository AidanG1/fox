using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int maxRicochets = 0;
    private int ricochets = 0;
    public int maxPierces = 0;
    private int pierces = 0;

    public float velocity = 10f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.forward * velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Trap"))
        {
            if (ricochets < maxRicochets)
            {
                // ricochet
                ricochets++;
            }
            else
            {
                // destroy
                Destroy(gameObject);
            }
        }
    }
}
