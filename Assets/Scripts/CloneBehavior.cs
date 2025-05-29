using UnityEngine;

public class CloneBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed = 5f;
    private PlayerMovement cloneMovement;
    private Vector2 currentDirection;
    private float directionTimer;

    // Initial movement
    private bool isInitialMovement = true;
    private float initialMovementTimer = 0.5f;
    private Vector2 initialDirection = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cloneMovement = GetComponent<PlayerMovement>();

        gameObject.GetComponent<PlayerVals>().setClone(true);
        cloneMovement.fallingColliderObject.SetActive(false);

        if (initialDirection != Vector2.zero)
        {
            currentDirection = initialDirection;
            SetRandomDirectionTimer();
        }
        else
            PickNewDirection();
    }

    void FixedUpdate()
    {
        directionTimer -= Time.fixedDeltaTime;
        if (directionTimer <= 0f)
            PickNewDirection();

        // Check for obstacles and map edges in the current direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, 1f);
        if (hit.collider != null)
        {
            // Check if it's a map edge or obstacle
            if (hit.collider.CompareTag("Fallable Prevent Walk Off") || hit.collider.CompareTag("Fallable"))
            {
                PickNewDirection();
            }
        }

        rb.linearVelocity = currentDirection * moveSpeed;
    }

    public void SetInitialDirection(Vector2 dir)
    {
        initialDirection = dir;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    void PickNewDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        currentDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        SetRandomDirectionTimer();
    }

    void SetRandomDirectionTimer()
    {
        directionTimer = Random.Range(0.2f, 3f);
    }
}