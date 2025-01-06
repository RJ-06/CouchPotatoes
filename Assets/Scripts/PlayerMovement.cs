using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerVals pv;
    [SerializeField] Rigidbody2D rb;
    Vector2 moveDir;

    PlayerInput playerInput;

    [SerializeField] float dashCooldownTimer;

    private void Start()
    {
        dashCooldownTimer = pv.getDashCooldown();
    }

    private void FixedUpdate()
    {
        Debug.Log(rb.linearVelocity.magnitude);
        dashCooldownTimer -= Time.fixedDeltaTime;
    }

    private void OnMovement(InputValue value) 
    {
        moveDir = value.Get<Vector2>().normalized;
        //if (Mathf.Sign(moveDir.x) == -Mathf.Sign(moveDir.x)) rb.linearVelocityX = 0;
        //if (Mathf.Sign(moveDir.y) == -Mathf.Sign(moveDir.y)) rb.linearVelocityY = 0;

        rb.linearVelocity = moveDir * pv.getMoveSpeed();
        ClampSpeed();
    }

    private void OnDash() 
    {
        if (dashCooldownTimer > 0) return;
        dashCooldownTimer = pv.getDashCooldown();

        rb.AddForce(moveDir * pv.getDashSpeed());
    }

    private void ClampSpeed() 
    {
        if (rb.linearVelocity.magnitude > pv.getMaxSpeed()) 
        {
            rb.linearVelocity = rb.linearVelocity.normalized * pv.getMaxSpeed();
        }
    }

    


}
