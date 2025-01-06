using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerVals pv;
    [SerializeField] Rigidbody2D rb;
    Vector2 movement;

    PlayerInput playerInput;

    private void Start()
    {

    }

    private void OnMovement(InputValue value) 
    {
        movement = value.Get<Vector2>();
    }

    private void OnDash() 
    {
        Debug.Log(pv.getMoveSpeed());
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * pv.getMoveSpeed();
        //Debug.Log(movement + "; " + rb.linearVelocity);
    }


}
