using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D ballRb;
    Vector3 lastVelocity;
    private void Start()
    {
        ballRb = GetComponent<Rigidbody2D>();
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        ballRb.velocity = direction * speed;
    }

    private void Update()
    {
        lastVelocity = ballRb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        ballRb.velocity = direction * Mathf.Max(speed, this.speed);
    }
}