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
    [SerializeField] float dashTimer;
    private bool dashInProgress = false;
    private Transform shockwaveItem;
    [SerializeField] float shockwaveCooldown;
    [SerializeField] float shockwaveCooldownTimer;
    private bool shockwaveUsed = false;
    [SerializeField] GameObject ShockwavePrefab;
    [SerializeField] PlayerPotato player;

    private void Start()
    {
        dashCooldownTimer = pv.getDashCooldown();
        dashTimer = pv.getDashTime();
        shockwaveCooldown = pv.getAttackCooldown();
    }

    private void FixedUpdate()
    {
        // Debug.Log(rb.linearVelocity.magnitude);
        dashCooldownTimer -= Time.fixedDeltaTime;

        if (dashInProgress && Mathf.Abs(dashTimer) >= 0.0001) {
            dashTimer -= Time.fixedDeltaTime;
        } else if (dashInProgress) {
            rb.AddForce(-1 * moveDir * pv.getDashSpeed());
            dashInProgress = false;
            dashTimer = pv.getDashTime();
        }

        if (shockwaveUsed && Mathf.Abs(shockwaveCooldownTimer) >= 0.0001) {
            shockwaveCooldownTimer -= Time.fixedDeltaTime;
        } else if (shockwaveUsed) {
            shockwaveUsed = false;
        }
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
        dashInProgress = true;
    }

    private void OnAttack()
    {
        shockwaveItem = transform.Find("ShockwaveItem(Clone)");

        if(shockwaveItem != null && !shockwaveUsed)
        {
            shockwaveUsed = true;
            shockwaveCooldownTimer = shockwaveCooldown;
            Instantiate(ShockwavePrefab, transform.position, Quaternion.identity, this.transform);
            Debug.Log("Attacked");
        }
    }

    private void OnInteract()
    {
        Debug.Log("Interacted");
        
    }

    private void ClampSpeed() 
    {
        if (rb.linearVelocity.magnitude > pv.getMaxSpeed()) 
        {
            rb.linearVelocity = rb.linearVelocity.normalized * pv.getMaxSpeed();
        }
    }

    private void OnStart()
    {
        int num = Random.Range(1, PlayerVals.numPlayers + 1);
    }


}
