using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPotato : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////

    // Game components
    [SerializeField] PlayerVals player;
    private PlayerMovement movement;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject potato;
    [SerializeField] GameObject potatoIndicator;
    [SerializeField] GameObject explosion;
    Explosion explodeScript;
    private GameManager gm;


    // Related to throwing and transferring the potato
    [InspectorLabel("Deal with holding potato to throw longer")]
    [SerializeField] float maxThrowForce;
    [SerializeField] float maxThrowTime;
    [SerializeField] float maxPotatoSpeed;
    private bool potatoThrown = false;
    
    private bool atPlayer = false;
    private Vector2 shootDir;


    // Events
    [Tooltip("Add in events")]
    public UnityEvent getPotato;
    [SerializeField] UnityEvent givePotato;


    // Potato bobbing :D
    private Vector2 bobOffset;
    private float xOffset = 0f, yOffset = 0f, initYOffset;
    private bool bobbing = false;
    private const float POTATO_FOLLOW_LERP_SPEED = 0.15f; // Smoothing factor for potato following

    [Header("---Sound Effects---")]
    [SerializeField] AudioSource playerSource;
    [SerializeField] AudioClip throwSound;
    [SerializeField] AudioClip catchSound;


    void Start()
    {
        playerSource = GetComponent<AudioSource>();
        explodeScript = explosion.GetComponent<Explosion>();
        movement = GetComponent<PlayerMovement>();
        gm = FindAnyObjectByType<GameManager>();
        gm.players.Add(gameObject);
    }

    void FixedUpdate()
    {
        if (!potatoThrown)
        {
            if (!bobbing) StartCoroutine(BobUpAndDown());
            bobbing = true;

            // Calculate target position for the potato to follow
            Vector2 playerPos = transform.position;
            Vector2 targetOffset = new Vector2(0, 0.75f); // Fixed offset behind player
            Vector2 targetPosition = playerPos + targetOffset + bobOffset;

            // Smoothly lerp the potato to follow the player
            potato.transform.position = Vector2.Lerp(potato.transform.position, targetPosition, POTATO_FOLLOW_LERP_SPEED);
            rb.linearVelocity = Vector2.zero;
        }
        else 
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxPotatoSpeed);
        }
            

    }


    ////////////////////////////
    ////////// INPUTS //////////
    ////////////////////////////

    private void OnThrow()
    {
        if (!potatoThrown && player.getHasPotato())
        {
            playerSource.clip = throwSound;
            playerSource.Play();
            if (shootDir != Vector2.zero) // Use right stick direction if given
            {
                rb.AddForce(maxThrowForce * shootDir);
            }
            else // Use previous direction moved if the potato isn't aimed
            {
                rb.AddForce(maxThrowForce * movement.lastMoveDir);
            }
            StartCoroutine(ReturnToPlayer());
            potatoThrown = true;
        }
    }

    private void OnAim(InputValue val) // Aim the potato with the right joystick on controller
    {
        shootDir = val.Get<Vector2>();
    }


    /////////////////////////////////////
    ////////// POTATO BEHAVIOR //////////
    /////////////////////////////////////

    private IEnumerator BobUpAndDown()
    {
        while (true)
        {
            for (float i = 1f; i <= 100f; ++i)
            {
                // Potato bobs up and down with an offset determined by a sine wave
                bobOffset = new Vector2(0, 0.15f * Mathf.Sin(0.02f * Mathf.PI * i));
                if (potatoThrown)  // Stop bobbing when thrown
                {
                    bobbing = false;
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    private IEnumerator ReturnToPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        atPlayer = false;
        float mult = 50f;

        // Make potato "gravitationally" return to player until it is reasonably close
        float exp = 0f;
        while (!atPlayer)
        {
            Vector2 returnForce = (float)Math.Pow(2, exp) * mult * new Vector2(transform.position.x - potato.transform.position.x, transform.position.y + 1f - potato.transform.position.y).normalized;
            rb.linearVelocity = 0.8f * rb.linearVelocity;
            rb.AddForce(returnForce);
            exp += 0.15f;

            // Stop coroutine if potato transfers
            if (!player.getHasPotato()) yield break;
            if (Vector2.Distance(transform.position, potato.transform.position) <= 1.5f)
            {
                atPlayer = true;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
        rb.linearVelocity = Vector2.zero;
        potatoThrown = false;

        playerSource.clip = catchSound;
        playerSource.Play();
        yield break;
    }

    private IEnumerator SmoothReturn()
    {
        // No longer needed with lerp-based following
        yield break;
    }

    public void onGetPotato()
    {
        playerSource.clip = catchSound;
        playerSource.Play();

        player.setHasPotato(true);
        potato.SetActive(true);
        potatoIndicator.SetActive(true);
        potato.GetComponent<SpriteRenderer>().enabled = true;
        if (gm.time <= 5f) gm.IncrementTime(5f);  // Some extra time for the player to react
    }

    public void onGivePotato()
    {
        potatoThrown = false;
        player.setHasPotato(false);
        potato.SetActive(false);
        potatoIndicator.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Take this branch if the potato reaches its own player
        if (potatoThrown && other.CompareTag("Potato") && other.transform.parent.gameObject == transform.gameObject)
        {
            atPlayer = true;
        }
        // Take this branch if the potato reaches a player that is not its own (this will transfer the potato)
        else if (!player.getHasPotato() && other.CompareTag("Potato") && other.transform.parent.GetComponent<PlayerPotato>().GetPotatoThrown())
        {
            PlayerPotato giver = other.transform.parent.GetComponent<PlayerPotato>();

            float transferX = giver.potato.transform.position.x;
            float transferY = giver.potato.transform.position.y;
            GetComponent<PlayerPotato>().potato.transform.position = new Vector2(transferX, transferY);
            giver.givePotato.Invoke();
            getPotato.Invoke();
            StartCoroutine(SmoothReturn());
        }
    }

    public void ExplodePotato()
    {
        bobbing = false;
        explosion.SetActive(true);
        explodeScript.ResetAndExplode();
        
    }

    public GameObject Potato() => potato;

    public bool GetPotatoThrown() => potatoThrown;

    public void SetPotatoIndicator(bool state) {
        if (state) 
            potatoIndicator.SetActive(true);
        else
            potatoIndicator.SetActive(false);
    }
}
