using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveDir;

    private PlayerInput playerInput;
    private PlayerVals pv;
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private bool canMove = true;
    private bool pushed = false;
    private bool isPusher = false;

    private bool isDashing = false;
    private float dashSpeedMultiplier = 2.0f;
    private Vector2 pushedVelocity;


    [SerializeField] PlayerPotato player;

    private void Start()
    {
        pv = GetComponent<PlayerVals>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!pushed && moveDir == Vector2.zero && !isPusher)
        {
            Vector2 keep = Vector2.Dot(pushedVelocity, moveDir) * moveDir;
            pushedVelocity = keep + (pushedVelocity - keep) * Mathf.Pow(0.1f, Time.deltaTime);
            rb.linearVelocity = pushedVelocity;
        }

        Debug.Log("Velocity: " + rb.linearVelocity);
    }

    private void OnMovement(InputValue value)
    {
        if (canMove)
        {
            moveDir = value.Get<Vector2>().normalized;
            //if (Mathf.Sign(moveDir.x) == -Mathf.Sign(moveDir.x)) rb.linearVelocityX = 0;
            //if (Mathf.Sign(moveDir.y) == -Mathf.Sign(moveDir.y)) rb.linearVelocityY = 0;

            // rb.linearVelocity = moveDir * pv.getMoveSpeed();
            // ClampSpeed();

            ApplyMovementSpeed();
        }
    }


    private void ApplyMovementSpeed()   // Set current speed based on is dashing is triggered or not
    {
        float speed;

        if (isDashing)
            speed = pv.getMoveSpeed() * dashSpeedMultiplier;
        else
            speed = pv.getMoveSpeed();

        rb.linearVelocity = moveDir * speed;


        ClampSpeed();
    }

    private void ClampSpeed()
    {
        if (rb.linearVelocity.magnitude > pv.getMaxSpeed())
        {
            rb.linearVelocity = rb.linearVelocity.normalized * pv.getMaxSpeed();
        }
    }

    private void OnInteract()
    {
        Debug.Log("Interacted");

    }

    private void OnStart()
    {
        int num = Random.Range(1, PlayerVals.numPlayers + 1);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (moveDir != Vector2.zero){
            isPusher = true;
        }
        else
        {
            isPusher = false;
        }

        pushed = true;
        // Store pushed velocity
        pushedVelocity = rb.linearVelocity;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        pushed = false;
    }


    public Vector2 getMoveDir() => moveDir;
    public void SetCanMove(bool canMove) => this.canMove = canMove;

    public void setDashing(bool dashing)
    {
        isDashing = dashing;
        ApplyMovementSpeed();
    }

    public void ForceSpeedUpdate() => ApplyMovementSpeed();     // Allow external script to force an immediate speed update.


}
