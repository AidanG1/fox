using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 5;
    public float pauseAtPatrolPoint = 0.0f;
    public bool accelerate = true;
    public Transform[] patrolPoints;
    [HideInInspector] public Rigidbody2D rb2d;
    private Vector3[] patrolPositions;
    private int nextIndex = 0;
    private float pauseTimer;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        patrolPositions = new Vector3[patrolPoints.Length];
        for(int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPositions[i] = patrolPoints[i].position;
            Destroy(patrolPoints[i].gameObject);
        }
        patrolPoints = new Transform[0];
    }

    void Update()
    {
        Vector2 toTarget = patrolPositions[nextIndex] - transform.position;
        Vector2 toPreviousTarget = patrolPositions[(nextIndex-1+patrolPositions.Length)% patrolPositions.Length] - transform.position;
        if (toTarget.magnitude <= speed * Time.fixedDeltaTime)
        {
            nextIndex++;
            nextIndex %= patrolPositions.Length;
            pauseTimer = pauseAtPatrolPoint;
        }

        float distance1 = toTarget.magnitude;
        float distance2 = toPreviousTarget.magnitude;
        float acceleration = Mathf.Clamp(Mathf.Min(distance1, distance2), 0.1f, 1.0f);

        toTarget.Normalize();
        if (accelerate)
        {
            toTarget *= acceleration;
        }

        pauseTimer -= Time.deltaTime;
        if (pauseTimer <= 0.0f)
        {
            rb2d.velocity = toTarget * speed;
        } else
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    private void OnDrawGizmos()
    {
        if (patrolPoints.Length < 2) return;

        Color startColor = new Color(0.6f, 0.2f, 0.2f);
        Color endColor = new Color(1.0f, 0.0f, 0.0f);
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Gizmos.color = Color.Lerp(startColor, endColor, i / (float)patrolPoints.Length);
            Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[(i + 1) % patrolPoints.Length].position);
            Gizmos.DrawWireSphere(patrolPoints[i].position,0.5f);
        }
    }
}
