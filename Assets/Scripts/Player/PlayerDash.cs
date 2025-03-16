using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    // Player components
    private PlayerVals pv;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    // Dash related
    private float dashCooldownTimer;
    private float dashTimer;
    private bool dashInProgress = false;

    void Start()
    {
        pv = GetComponent<PlayerVals>();
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        dashCooldownTimer = pv.getDashCooldown();
        dashTimer = pv.getDashTime();
    }
    
    private void FixedUpdate()
    {
        dashCooldownTimer -= Time.fixedDeltaTime;

        if (dashInProgress && Mathf.Abs(dashTimer) >= 0.0001) {     // Dashing is in progress
            gameObject.GetComponent<PlayerMovement>().fallingColliderObject.SetActive(true); // Player can fall when dashing
            dashTimer -= Time.fixedDeltaTime;
        } else if (dashInProgress) {    // Dashing has ended as the timer <= 0
            playerMovement.SetDashing(false);
            dashInProgress = false;
            gameObject.GetComponent<PlayerMovement>().fallingColliderObject.SetActive(false);
            dashTimer = pv.getDashTime();
        }
    }

    private void OnDash() 
    {
        if (pv == null) return;  // Prevent null reference on controller join

        if (dashCooldownTimer > 0) return;  // > 0 meaning in cooldown, <= 0 meaning can be used 
 
        dashCooldownTimer = pv.getDashCooldown();
        dashInProgress = true;
        playerMovement.SetDashing(true);
        playerMovement.ForceSpeedUpdate();
    }
}
