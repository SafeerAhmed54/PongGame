using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D ballRb;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float speedIncrement = 0.1f;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TrailRenderer trailRenderer;

    private Vector3 lastVelocity;
    private float currentSpeed;

    private void Start()
    {
        ballRb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        currentSpeed = speed;
        StartCoroutine(LaunchBall());
    }

    private void Update()
    {
        lastVelocity = ballRb.velocity;

        // Gradually increase speed up to the maximum
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += speedIncrement * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

            // Apply the new speed while maintaining direction
            ballRb.velocity = ballRb.velocity.normalized * currentSpeed;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        // Prevent too flat horizontal trajectories
        if (Mathf.Abs(direction.y) < 0.2f)
        {
            direction.y = direction.y < 0 ? -0.2f : 0.2f;
            direction = direction.normalized;
        }

        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Add spin based on where the paddle was hit
            float hitFactor = (transform.position.y - collision.transform.position.y) /
                              collision.collider.bounds.size.y;
            direction.y += hitFactor * 0.5f;
            direction = direction.normalized;

            // Optional: Increase speed slightly on paddle hit
            currentSpeed += speedIncrement * 2;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }

        // Apply current speed to the new direction
        ballRb.velocity = direction * currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal1"))
        {
            Debug.Log("Goal 1 Scored");
            gameManager.AddScore(true);
            trailRenderer.widthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
            StartCoroutine(ResetBall());
        }
        else if (collision.gameObject.CompareTag("Goal2"))
        {
            Debug.Log("Goal 2 Scored");
            gameManager.AddScore(false);
            trailRenderer.widthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
            StartCoroutine(ResetBall());
        }
    }

    private IEnumerator ResetBall()
    {
        yield return new WaitForSeconds(2.0f);
        trailRenderer.widthCurve = new AnimationCurve(new Keyframe(0, 0.425f), new Keyframe(1, 0));
        transform.position = Vector3.zero; // Or your start position
        currentSpeed = speed; // Reset to initial speed
    }

    IEnumerator LaunchBall()
    {
        yield return new WaitForSeconds(1.0f);
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        ballRb.velocity = direction * currentSpeed;
    }
}