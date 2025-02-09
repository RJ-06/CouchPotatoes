using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashCooldownTimer;
    [SerializeField] float dashTimer;
    private bool dashInProgress = false;
    private float originalMovesSpeed;
    private float dashSpeedMultiplier = 2.0f;

    private PlayerVals pv;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PlayerVals>();
        dashCooldownTimer = pv.getDashCooldown();
        dashTimer = pv.getDashTime();
        playerMovement = GetComponent<PlayerMovement>();
        originalMovesSpeed = pv.getMoveSpeed();
    }
    
    private void FixedUpdate()
    {
        // Debug.Log(rb.linearVelocity.magnitude);
        dashCooldownTimer -= Time.fixedDeltaTime;

        if (dashInProgress && Mathf.Abs(dashTimer) >= 0.0001) {     // Dashing is on progress
            dashTimer -= Time.fixedDeltaTime;
        } else if (dashInProgress) {    // Dashing has end as the timer <= 0

            // rb.AddForce(-1 * playerMovement.getMoveDir() * pv.getDashSpeed());  // Apply opposite force to stop the dashing

            playerMovement.setDashing(false);
            dashInProgress = false;
            dashTimer = pv.getDashTime();
        }
    }

    private void OnDash() 
    {
        if (dashCooldownTimer > 0) return;      // > 0 meaning in cooldown, <= 0 meaning can be used 
 
        dashCooldownTimer = pv.getDashCooldown();
        dashInProgress = true;

        // rb.AddForce(playerMovement.getMoveDir() * pv.getDashSpeed());
        playerMovement.setDashing(true);
        playerMovement.ForceSpeedUpdate();
    }
}
