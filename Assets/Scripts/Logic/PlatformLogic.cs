using System.Collections;
using UnityEngine;

public class PlatformLogic : MonoBehaviour
{
    public GameObject startPoint; // First endpoint
    public GameObject endPoint; // Second endpoint
    public float travelTime = 3.0f; // How long it takes for platform to go from one side to the other
    public float pauseTime = 1.0f; // How long platform pauses at end point
    private Vector2 startPosition; // First endpoint
    private Vector2 endPosition; // Second endpoint
    private float timer; // Timer for platform
    private Rigidbody2D rb; // RigidBody2D of the platform
    private PlatformState state; // Current state of the platform

    // States meant to handle direction changes and pausing of platforms
    private enum PlatformState
    {
        MovingToEnd,
        PausedAtEnd,
        MovingToStart,
        PausedAtStart
    }

    void Start()
    {
        // Set private instance variables
        startPosition = startPoint.transform.position;
        endPosition = endPoint.transform.position;
        timer = 0;
        rb = GetComponent<Rigidbody2D>();
        state = PlatformState.MovingToEnd;

        // Check to see if durations are not negative
        if (travelTime <= 0)
        {
            Debug.LogWarning("Travel time must be positive. Defaulting to 3 seconds.");
            travelTime = 3.0f;
        }

        if (pauseTime < 0)
        {
            Debug.LogWarning("Pause time must be nonnegative. Defaulting to 0 seconds.");
            pauseTime = 0.0f;
        }
    }

    void FixedUpdate()
    {
        // Check current state of platform
        switch (state)
        {
            // Move towards end point
            case PlatformState.MovingToEnd:
                MoveTowards(true, PlatformState.MovingToStart);
                break;
            // Move towards start point
            case PlatformState.MovingToStart:
                MoveTowards(false, PlatformState.MovingToEnd);
                break;
            // Paused at end point
            case PlatformState.PausedAtEnd:
                // Do nothing
                break;
            // Paused at start point
            case PlatformState.PausedAtStart:
                
                // Do nothing
                break;
            // How did we get here?
            default:
                // I don't know how you'd get here but log it and do nothing
                Debug.LogWarning("Somehow broke platform state machine...");
                break;
        }
    }

    // Method for moving the platform
    // "end" represents whether the platform is moving towards endPosition or not
    // "nextState" represents the next moving state that the platform will be in
    void MoveTowards(bool end, PlatformState nextState)
    {
        Vector2 targetPos = end ? endPosition : startPosition;
        Vector2 currentPos = rb.position;
        Vector2 direction = (targetPos - currentPos).normalized;
        // Update timer
        timer += Time.fixedDeltaTime;

        float distance = Vector2.Distance(currentPos, targetPos);
        float speed = Vector2.Distance(startPosition, endPosition) / travelTime;
        // Make a smooth step based on the timer
        float t = Mathf.SmoothStep(0.0f, 1.0f, timer / travelTime);

        // Calculate the position the platform is supposed to be in
        Vector3 newPosition = Vector3.Lerp(
            end ? startPosition : endPosition,
            end ? endPosition : startPosition,
            t
        );
        //rb.linearVelocity = direction * speed;

        // Move the platform based on which direction we are moving

        rb.MovePosition(newPosition);

        // Reset timer after "travelTime" seconds, pause for "pauseTime" seconds, then move other way
        
        if (timer >= travelTime)
        {
            timer = 0;
            StartCoroutine(PauseThenSwitch(nextState));
        }
        /*
        if (distance <= speed*Time.fixedDeltaTime)
        {
            rb.position = targetPos;
            rb.linearVelocity = Vector2.zero;
            timer = 0;
            StartCoroutine(PauseThenSwitch(nextState));
        }
        */
    }

    // Method for pausing the platform at an endpoint then switching directions
    // "nextState" represents the direction platform will move after pausing
    IEnumerator PauseThenSwitch(PlatformState nextState)
    {
        // Set moving state to respective pausing state
        state = (state == PlatformState.MovingToEnd)
            ? PlatformState.PausedAtEnd
            : PlatformState.PausedAtStart
        ;

        // Wait for the pause duration
        yield return new WaitForSeconds(pauseTime);

        // Set state to next moving state
        state = nextState;
    }
}
