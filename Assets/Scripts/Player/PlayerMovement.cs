using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    // Player components
    private PlayerVals pv;
    private Rigidbody2D rb;
    private PlayerPotato potato;
    public GameObject fallingColliderObject;  // Child of player that turns on a collider when a player is vulnerable to falling

    // Related directly to player movement
    private bool canMove = true;
    public Vector2 lastMoveDir = new Vector2(0, -1);
    Vector2 moveDir;

    // Players pushing other players
    private bool pushed = false;
    private bool isPusher = false;
    private Vector2 pushedVelocity;

    // Other effects on player movement
    private bool fallInProgress = false;
    private bool isDashing = false;
    private bool hitByShockwave = false;
    private float dashSpeedMultiplier = 2.0f;

    private void Start() {
        pv = GetComponent<PlayerVals>();
        rb = GetComponent<Rigidbody2D>();
        potato = GetComponent<PlayerPotato>();
    }

    private void FixedUpdate()
    {
        if (hitByShockwave) {
            fallingColliderObject.SetActive(true);  // Allow player to fall if they are hit off by a shockwave
            pushedVelocity = rb.linearVelocity;
            ExecutePush(0.2f);
        } else if (!pushed && moveDir == Vector2.zero && !isPusher) {
            fallingColliderObject.SetActive(true);
            pushedVelocity = rb.linearVelocity; // Store pushed velocity before executing the push
            ExecutePush(0.7f);
        // If player moves make sure push force is canceled
        } else if (moveDir != Vector2.zero) {
            pushedVelocity = Vector2.zero;
        }
    }


    ////////////////////////////
    ////////// INPUTS //////////
    ////////////////////////////

    private void OnStart(InputValue value)
    {
        FindAnyObjectByType<GameManager>().StartGame();
    }

    private void OnMovement(InputValue value)
    {
        if (canMove)
        {            
            // Move player
            if(value.Get<Vector2>() != new Vector2(0, 0))
            {
                moveDir = value.Get<Vector2>().normalized;
                lastMoveDir = value.Get<Vector2>().normalized;
            }
            else moveDir = new Vector2(0, 0);

            ApplyMovementSpeed();
        }
    }


    //////////////////////////////
    ////////// MOVEMENT //////////
    //////////////////////////////
    
    private void ApplyMovementSpeed()  // Set current speed based on whether dashing is triggered or not
    {
        float speed;

        if (isDashing)
            speed = pv.getMoveSpeed() * dashSpeedMultiplier;
        else
            speed = pv.getMoveSpeed();

        rb.linearVelocity = moveDir * speed;

        ClampSpeed();
    }

    private void ClampSpeed() // Make sure the player doesn't go over the max speed while moving
    {
        if (rb.linearVelocity.magnitude > pv.getMaxSpeed())
        {
            rb.linearVelocity = rb.linearVelocity.normalized * pv.getMaxSpeed();
        }
    }

    public void ForceSpeedUpdate() => ApplyMovementSpeed();  // Allow external script to force an immediate speed update.

    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
        if (!canMove) moveDir = Vector2.zero;
    }

    public Vector2 getMoveDir() => moveDir;


    ////////////////////////////////
    ////////// COLLISIONS //////////
    ////////////////////////////////
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Set up pushing effect when colliding with other players
        if (moveDir != Vector2.zero && other.gameObject.CompareTag("Player")){
            isPusher = true;
        }
        else
        {
            isPusher = false;
        }

        pushed = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        pushed = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Fallable") && !fallInProgress) {
            fallInProgress = true;
            StartCoroutine(Fall());
        }
    }


    /////////////////////////////
    ////////// FALLING //////////
    /////////////////////////////
    
    public IEnumerator Fall()
    {
        SetCanMove(false);
        rb.linearVelocity = Vector2.zero;

        // Shrink the player to simulate a falling effect
        while (gameObject.transform.localScale.x >= 0.01f) {
            gameObject.transform.localScale *= 0.8f;
            yield return new WaitForSeconds(0.05f);
        }
        // Additional 1.5 second punishment, because why have mercy on a player who was stupid enough to fall
        yield return new WaitForSeconds(1.5f);

        // Restore the player at a respawn point with the loss of some health
        gameObject.transform.localScale = new Vector2(1f, 1f);
        gameObject.GetComponent<PlayerVals>().setHealth((int)gameObject.GetComponent<PlayerVals>().getHealth() / 2);
        gameObject.transform.position = PickRespawnPoint();
        fallInProgress = false;
        SetCanMove(true);
    }

    private Vector2 PickRespawnPoint()
    {
        // List of respawn points is picked inside editor...might change this if a better solution arises
        List<Vector2> choices = FindAnyObjectByType<GameManager>().getRespawnPoints();
        Vector2 respawnPoint = choices[0];
        float respawnDist = Utilities.FindDistance(transform.position, respawnPoint);

        // Choose the respawn point closes to the fall, return that
        for(int i = 1; i < choices.Count; ++i) {
            if (Utilities.FindDistance(transform.position, choices[i]) < respawnDist) {
                respawnPoint = choices[i];
                respawnDist = Utilities.FindDistance(transform.position, respawnPoint);
            }
        }
        return respawnPoint;
    }

    public bool GetFallInProgress() => fallInProgress;


    ///////////////////////////
    ////////// OTHER //////////
    ///////////////////////////
    
    // Allow a player to be pushed and retain some momentum, gradually coming to a stop
    // Lower stopFactor equates to a faster stop, stopFactor < 1f (or the player will speed up)
    private void ExecutePush(float stopFactor)
    {
        // Slow down the player gradually
        pushedVelocity *= stopFactor;
        rb.linearVelocity = pushedVelocity;

        if (rb.linearVelocity.magnitude <= 0.001f) {
            fallingColliderObject.SetActive(false);
            hitByShockwave = false;
        }
    }

    public void SetDashing(bool dashing)
    {
        isDashing = dashing;
        ApplyMovementSpeed();
    }

    public void SetHitByShockwave(bool b) => hitByShockwave = b;

    public bool GetHitByShockwave() => hitByShockwave;
}
