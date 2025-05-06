using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 5f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float speedIncrement = 0.25f;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip scoreSound;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Vector2 startPosition;
    private float currentSpeed;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        startPosition = transform.position;
        gameManager = FindObjectOfType<GameManager>();

        // Start the game after a short delay
        StartCoroutine(StartBall());
    }

    IEnumerator StartBall()
    {
        rb.velocity = Vector2.zero;
        transform.position = startPosition;
        currentSpeed = initialSpeed;

        // Wait before launching the ball
        yield return new WaitForSeconds(1.5f);

        // Launch in a random direction
        float randomDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        float randomAngle = Random.Range(-0.3f, 0.3f);
        Vector2 direction = new Vector2(randomDirection, randomAngle).normalized;

        rb.velocity = direction * currentSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Play hit sound
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // Get collision normal
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 normal = contact.normal;

        // Reflect the ball
        Vector2 newDirection = Vector2.Reflect(rb.velocity, normal).normalized;

        // Increase speed slightly with each hit, up to max speed
        currentSpeed = Mathf.Min(currentSpeed + speedIncrement, maxSpeed);

        // Apply the new velocity
        rb.velocity = newDirection * currentSpeed;

        // Add a bit of spin based on where the paddle was hit (if it was a paddle)
        if (collision.gameObject.CompareTag("Paddle"))
        {
            float hitFactor = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
            newDirection.y += hitFactor * 0.75f;
            rb.velocity = newDirection.normalized * currentSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ball entered a goal area
        if (collision.CompareTag("Goal1"))
        {
            // Player 2 scored
            if (scoreSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(scoreSound);
            }
            gameManager.AddScore(false);
            StartCoroutine(StartBall());
        }
        else if (collision.CompareTag("Goal2"))
        {
            // Player 1 scored
            if (scoreSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(scoreSound);
            }
            gameManager.AddScore(true);
            StartCoroutine(StartBall());
        }
    }
}