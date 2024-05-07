using UnityEngine;

public class OscillatingPlatform : MonoBehaviour
{
    public Transform startPoint; // Starting point of the oscillating platform
    public Transform endPoint; // Ending point of the oscillating platform
    public float speed = 2f; // Speed at which the platform moves
    public float waitTime = 0.5f; // Time to wait at each point

    private float waitTimer = 0f;
    private bool movingToEnd = true; // Flag to indicate whether the platform is moving towards the end point

    private void Start()
    {
        transform.position = startPoint.position;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        Transform targetPoint = movingToEnd ? endPoint : startPoint;

        // Move towards the target point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Check if the platform has reached the target point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            // Increase the wait timer
            waitTimer += Time.deltaTime;

            // Check if the platform should wait at the current point
            if (waitTimer >= waitTime)
            {
                // Reset the wait timer
                waitTimer = 0f;

                // Change direction
                movingToEnd = !movingToEnd;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.WasWithPlayer())
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.WasWithPlayer())
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
