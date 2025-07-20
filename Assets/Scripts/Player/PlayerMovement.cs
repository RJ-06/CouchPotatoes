using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////

    // Player components
    private PlayerVals pv;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private PlayerGateCheck gc;
    private PlayerPotato potato;
    public GameObject fallingColliderObject;  // Child of player that turns on a collider when a player is vulnerable to falling
    public GameObject fallingAlwaysColliderObject;
    private bool velocityOverride = false;
    private bool isAlive = true;

    // Related directly to player movement
    private bool canMove = true;
    public Vector2 lastMoveDir = new Vector2(0, -1);
    private Vector2 moveDir;
    private Vector2 offsetVelocity = Vector2.zero;

    // Players pushing other players
    private bool pushed = false;
    private bool isPusher = false;
    private Vector2 pushedVelocity;

    // Other effects on player movement
    private bool fallInProgress = false;
    private bool isDashing = false;
    private bool hitByShockwave = false;
    private bool insidePlatform = false;
    private float dashSpeedMultiplier = 2.0f;

    // Gate related stuff
    private bool onLandGate = false, onMovingGate = false;

    [Header("---SOUND EFFECTS---")]
    [SerializeField] AudioSource playerSource;
    [SerializeField] AudioClip dashSound;



    private void Start()
    {
        //playerSource = GetComponent<AudioSource>();
        pv = GetComponent<PlayerVals>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        gc = GetComponentInChildren<PlayerGateCheck>();
        potato = GetComponent<PlayerPotato>();
    }

    private void FixedUpdate()
    {
        if (!velocityOverride)
        {
            ApplyMovementSpeed();
        }

        if (hitByShockwave)
        {
            fallingColliderObject.SetActive(true);  // Allow player to fall if they are hit off by a shockwave
            pushedVelocity = rb.linearVelocity;
            ExecutePush(0.2f);
        }
        else if (!pushed && moveDir == Vector2.zero && !isPusher && !pv.getClone() && !gc.GetInGate() && !insidePlatform)
        {
            fallingColliderObject.SetActive(true);
            pushedVelocity = rb.linearVelocity; // Store pushed velocity before executing the push
            ExecutePush(0.7f);
            // If player moves make sure push force is canceled
        }
        else if (moveDir != Vector2.zero)
        {
            pushedVelocity = Vector2.zero;
        }

        if ((!onLandGate || !onMovingGate) && isAlive && !fallInProgress) gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }


    ////////////////////////////
    ////////// INPUTS //////////
    ////////////////////////////

    private void OnStart()
    {
        if (!FindAnyObjectByType<GameManager>().GetFirstGameStarted())
            FindAnyObjectByType<GameManager>().StartGame();
    }

    private void OnPause()
    {
        FindAnyObjectByType<GameManager>().GetPauseScript().Pause();
        FindAnyObjectByType<GameManager>().GetPauseScript().optionsMenu.GetComponent<OptionsManager>().SetPlayerThatPaused(pv);
    }

    private void OnMovement(InputValue value)
    {
        if (canMove)
        {
            velocityOverride = false;

            // Move player
            if (value.Get<Vector2>() != new Vector2(0, 0))
            {
                Debug.Log(pv.getMovementMultiplier());
                Debug.Log(pv.getSpeedSensitivityMultiplier());
                Debug.Log(value.Get<Vector2>());
                moveDir = pv.getMovementMultiplier() * pv.getSpeedSensitivityMultiplier() * value.Get<Vector2>();
                lastMoveDir = value.Get<Vector2>().normalized;
            }
            else moveDir = new Vector2(0, 0);

            ApplyMovementSpeed();
        }
    }


    //////////////////////////////
    ////////// MOVEMENT //////////
    //////////////////////////////

    public void ApplyMovementSpeed()  // Set current speed based on whether dashing is triggered or not
    {
        if (pv == null) return;  // Prevent null reference on controller join

        float speed;
        if (isDashing)
            speed = pv.getMoveSpeed() * dashSpeedMultiplier;
        else
            speed = pv.getMoveSpeed();

        rb.linearVelocity = moveDir * speed;

        ClampSpeed();

        // Account for velocities not from the player (e.g. a moving platform)
        if (insidePlatform)
        {
            rb.linearVelocity += offsetVelocity;
        }
    }

    private void ClampSpeed() // Make sure the player doesn't go over the max speed while moving
    {
        if (!isDashing && (rb.linearVelocity.magnitude > pv.getMoveSpeed()))
        {
            rb.linearVelocity = rb.linearVelocity.normalized * pv.getMoveSpeed();
        }
        else if (rb.linearVelocity.magnitude > pv.getMaxSpeed())
        {
            rb.linearVelocity = rb.linearVelocity.normalized * pv.getMaxSpeed();
        }
    }

    public void ForceSpeedUpdate() => ApplyMovementSpeed();  // Allow external script to force an immediate speed update.

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
        if (!canMove) moveDir = Vector2.zero;
    }

    public Vector2 getMoveDir() => moveDir;
    public void setMoveDir(Vector2 newMoveDir) => moveDir = newMoveDir;


    ////////////////////////////////
    ////////// COLLISIONS //////////
    ////////////////////////////////

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Set up pushing effect when colliding with other players
        if (moveDir != Vector2.zero && other.gameObject.CompareTag("Player"))
        {
            isPusher = true;
        }
        else
        {
            isPusher = false;
        }

        pushed = true;

        // Deal with areas that always cause falling
        if (other.gameObject.CompareTag("Fallable Always"))
        {
            fallingAlwaysColliderObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        pushed = false;

        // Deal with areas that always cause falling
        if (other.gameObject.CompareTag("Fallable Always"))
        {
            fallingAlwaysColliderObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Fallable") && !fallInProgress)
        {
            fallInProgress = true;
            StartCoroutine(Fall());
        }
        /*else if (other.gameObject.CompareTag("Fallable Always") && !fallInProgress)
        {
            fallingAlwaysColliderObject.SetActive(true);
            fallInProgress = true;
            StartCoroutine(Fall());
        }*/

        if (other.gameObject.CompareTag("Land Gate"))
        {
            onLandGate = true;
        }
        if (other.gameObject.CompareTag("Moving Platform Gate"))
        {
            onMovingGate = true;
        }
    }


    /////////////////////////////
    ////////// FALLING //////////
    /////////////////////////////

    public IEnumerator Fall()
    {
        SetCanMove(false);
        rb.linearVelocity = Vector2.zero;
        bc.enabled = false;

        // Move the player smoothly to the nearest fall point before doing the animation
        Vector2 fallPosition = Utilities.FindNearestTileInSet(transform.position, FindAnyObjectByType<GameManager>().GetFallPoints());
        Debug.Log("Fall position: " + fallPosition);
        while (Utilities.FindDistance(transform.position, fallPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, fallPosition, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }

        // Shrink the player to simulate a falling effect
        while (gameObject.transform.localScale.x >= 0.01f)
        {
            gameObject.transform.localScale *= 0.8f;
            yield return new WaitForSeconds(0.05f);
        }
        // Additional 1.5 second punishment, because why have mercy on a player who was stupid enough to fall
        yield return new WaitForSeconds(1.5f);

        // Restore the player at a respawn point with the loss of some health
        gameObject.transform.localScale = new Vector2(1f, 1f);
        gameObject.GetComponent<PlayerVals>().IncrementHealth(-1 * (int)gameObject.GetComponent<PlayerVals>().getMaxHealth() / 4);
        Debug.Log(PickRespawnPoint());
        gameObject.transform.position = PickRespawnPoint();
        fallInProgress = false;
        bc.enabled = true;
        Debug.Log("Error possible in PlayerMovement");
        SetCanMove(true);
    }

    private Vector2 PickRespawnPoint()
    {
        Tilemap choices = FindAnyObjectByType<GameManager>().GetRespawnPoints();
        Vector2 currentPos = transform.position;
        Vector2 respawnPoint = Utilities.FindNearestTileInSet(currentPos, choices);

        return respawnPoint;
    }

    public bool GetFallInProgress() => fallInProgress;
    public void SetFallInProgress(bool state) => fallInProgress = state;


    ///////////////////////////
    ////////// OTHER //////////
    ///////////////////////////

    // Allow a player to be pushed and retain some momentum, gradually coming to a stop
    // Lower stopFactor equates to a faster stop, stopFactor < 1f (or the player will speed up)
    public void ExecutePush(float stopFactor)
    {
        // Slow down the player gradually
        pushedVelocity *= stopFactor;
        rb.linearVelocity = pushedVelocity;

        if (rb.linearVelocity.magnitude <= 0.001f)
        {
            fallingColliderObject.SetActive(false);
            hitByShockwave = false;
            velocityOverride = false;
        }
    }

    public void SetDashing(bool dashing)
    {
        if (dashing)
        {
            playerSource.clip = dashSound;
            playerSource.Play();
        }
        isDashing = dashing;
        ApplyMovementSpeed();
    }

    public void SetHitByShockwave(bool b) => hitByShockwave = b;

    public bool GetHitByShockwave() => hitByShockwave;

    public bool GetOnLandGate() => onLandGate;
    public void SetOnLandGate(bool state) => onLandGate = state;
    public bool GetOnMovingGate() => onMovingGate;
    public void SetOnMovingGate(bool state) => onMovingGate = state;

    public Vector2 GetOffsetVelocity() => offsetVelocity;
    public void SetOffsetVelocity(Vector2 velocity) => offsetVelocity = velocity;

    public bool GetInsidePlatform() => insidePlatform;
    public void SetInsidePlatform(bool state) => insidePlatform = state;

    public void SetVelocityOverride(bool state) => velocityOverride = state;

    public void SetPushedVelocity(Vector2 velocity) => pushedVelocity = velocity;

    public bool GetAlive() => isAlive;
    public void SetAlive(bool state) => isAlive = state;
}
