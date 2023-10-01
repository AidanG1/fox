using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int maxRicochets = 0;
    private int ricochets = 0;
    public int maxPierces = 0;
    private int pierces = 0;

    public void Shoot(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Ground") || (collision.gameObject.tag == "Trap"))
        {
            Destroy(gameObject);
        }
    }
}
