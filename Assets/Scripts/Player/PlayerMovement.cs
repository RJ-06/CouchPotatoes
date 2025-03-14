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
    public Vector2 lastMoveDir;
    Vector2 moveDir;

    private PlayerInput playerInput;
    private PlayerVals pv;
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private bool canMove = true;
    private bool pushed = false;
    private bool isPusher = false;

    private bool isDashing = false;
    private bool hitByShockwave = false;
    private bool fallInProgress = false;
    private float dashSpeedMultiplier = 2.0f;
    private Vector2 pushedVelocity;


    [SerializeField] PlayerPotato player;

    private void Start()
    {
        pv = GetComponent<PlayerVals>();
        rb = GetComponent<Rigidbody2D>();
        lastMoveDir = new Vector2(0, -1);
    }

    private void FixedUpdate()
    {
        if (hitByShockwave) {
            pushedVelocity = rb.linearVelocity;
            ExecutePush(0.2f);
            Debug.Log("Shockwave push done");
        } else if (!pushed && moveDir == Vector2.zero && !isPusher) {
            // Store pushed velocity before executing the push
            pushedVelocity = rb.linearVelocity;
            ExecutePush(0.7f);
        // If player moves make sure push force is canceled
        } else if (moveDir != Vector2.zero) {
            pushedVelocity = Vector2.zero;
        }
    }

    private void OnStart(InputValue value)
    {
        Debug.Log(FindAnyObjectByType<GameManager>());
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
            Debug.Log("Fell");
            fallInProgress = true;
            StartCoroutine(Fall());
        }
    }

    // Lower stopFactor equates to a faster stop, stopFactor < 1f
    private void ExecutePush(float stopFactor)
    {
        Vector2 keep = Vector2.Dot(pushedVelocity, moveDir) * moveDir;
        pushedVelocity *= stopFactor;
        rb.linearVelocity = pushedVelocity;

        if (rb.linearVelocity.magnitude <= 0.001f) {
            hitByShockwave = false;
        }
    }


    public Vector2 getMoveDir() => moveDir;
    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
        if (!canMove) moveDir = Vector2.zero;
    }

    public bool GetHitByShockwave() => hitByShockwave;

    public void SetHitByShockwave(bool b) => hitByShockwave = b;

    public IEnumerator Fall()
    {
        SetCanMove(false);
        Vector2 lastVelocity = rb.linearVelocity.normalized;
        rb.linearVelocity = Vector2.zero;
        while (gameObject.transform.localScale.x >= 0.01f) {
            gameObject.transform.localScale *= 0.8f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1.5f);
        gameObject.transform.localScale = new Vector2(1f, 1f);
        gameObject.GetComponent<PlayerVals>().setHealth((int)gameObject.GetComponent<PlayerVals>().getHealth() / 2);
        gameObject.transform.position = pickRespawnPoint();
        fallInProgress = false;
        SetCanMove(true);
    }

    public bool getFallInProgress() => fallInProgress;

    public void setDashing(bool dashing)
    {
        isDashing = dashing;
        ApplyMovementSpeed();
    }

    public void ForceSpeedUpdate() => ApplyMovementSpeed();     // Allow external script to force an immediate speed update.

    private Vector2 pickRespawnPoint()
    {
        List<Vector2> choices = FindAnyObjectByType<GameManager>().getRespawnPoints();
        Vector2 respawnPoint = choices[0];
        float respawnDist = findDistance(transform.position, respawnPoint);
        for(int i = 1; i < choices.Count; ++i) {
            if (findDistance(transform.position, choices[i]) < respawnDist) {
                respawnPoint = choices[i];
                respawnDist = findDistance(transform.position, respawnPoint);
            }
        }

        Debug.Log(respawnPoint);
        return respawnPoint;
    }

    private float findDistance(Vector2 a, Vector2 b) {
        float xDist = Mathf.Abs(a.x - b.x);
        float yDist = Mathf.Abs(a.y - b.y);
        return Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2));
    }
}
