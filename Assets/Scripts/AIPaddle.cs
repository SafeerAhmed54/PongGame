using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float reactionDelay = 0.1f;
    [SerializeField] private float paddleBoundaryTop = 4.5f;
    [SerializeField] private float paddleBoundaryBottom = -4.5f;
    [SerializeField] private float randomOffset = 0.5f;

    private GameObject ball;
    private Vector2 targetPosition;
    private float lastUpdateTime;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        lastUpdateTime = Time.time;
    }

    void Update()
    {
        // Only update target position after delay (to simulate human reaction time)
        if (Time.time >= lastUpdateTime + reactionDelay)
        {
            if (ball != null)
            {
                // Update target Y position based on ball position with some randomness
                float randomFactor = Random.Range(-randomOffset, randomOffset);
                targetPosition.y = ball.transform.position.y + randomFactor;
                lastUpdateTime = Time.time;
            }
        }

        // Move paddle toward target position
        Vector2 currentPosition = transform.position;
        currentPosition.y = Mathf.MoveTowards(
            currentPosition.y,
            targetPosition.y,
            moveSpeed * Time.deltaTime
        );

        // Clamp position within screen boundaries
        currentPosition.y = Mathf.Clamp(currentPosition.y, paddleBoundaryBottom, paddleBoundaryTop);

        // Apply the movement
        transform.position = currentPosition;
    }
}