using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveDir;
    
    private PlayerInput playerInput;
    private PlayerVals pv;
    private Rigidbody2D rb;
    
    [SerializeField] PlayerPotato player;
    
    private void Start()
    {
        pv = GetComponent<PlayerVals>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMovement(InputValue value) 
    {
        moveDir = value.Get<Vector2>().normalized;
        //if (Mathf.Sign(moveDir.x) == -Mathf.Sign(moveDir.x)) rb.linearVelocityX = 0;
        //if (Mathf.Sign(moveDir.y) == -Mathf.Sign(moveDir.y)) rb.linearVelocityY = 0;

        rb.linearVelocity = moveDir * pv.getMoveSpeed();
        ClampSpeed();
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

    public Vector2 getMoveDir() => moveDir;

}
