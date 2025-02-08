using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] float dashCooldownTimer;
    [SerializeField] float dashTimer;
    private bool dashInProgress = false;

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
    }
    
    private void FixedUpdate()
    {
        // Debug.Log(rb.linearVelocity.magnitude);
        dashCooldownTimer -= Time.fixedDeltaTime;

        if (dashInProgress && Mathf.Abs(dashTimer) >= 0.0001) {
            dashTimer -= Time.fixedDeltaTime;
        } else if (dashInProgress) {
            rb.AddForce(-1 * playerMovement.getMoveDir() * pv.getDashSpeed());
            dashInProgress = false;
            dashTimer = pv.getDashTime();
        }
    }

    private void OnDash() 
    {
        if (dashCooldownTimer > 0) return;
        dashCooldownTimer = pv.getDashCooldown();

        rb.AddForce(playerMovement.getMoveDir() * pv.getDashSpeed());
        dashInProgress = true;
    }
}
