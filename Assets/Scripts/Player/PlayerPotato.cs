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
    private bool potatoThrown = false;
    private bool atPlayer = false;
    private Vector2 shootDir;


    // Events
    [Tooltip("Add in events")]
    public UnityEvent getPotato;
    [SerializeField] UnityEvent givePotato;


    // Potato bobbing :D
    private Vector2 bobOffset;
    private float xOffset = 0f, yOffset = 0f, initYOffset, xShift, yShift;
    private bool bobbing = false;

    [Header("---Sound Effects---")]
    [SerializeField] AudioSource playerSource;
    [SerializeField] AudioClip throwSound;
    [SerializeField] AudioClip catchSound;


    void Start()
    {
        playerSource = GetComponent<AudioSource>();
        explodeScript = explosion.GetComponent<Explosion>();
        movement = GetComponent<PlayerMovement>();
        gm = FindFirstObjectByType<GameManager>();
        gm.players.Add(gameObject);
    }

    void FixedUpdate()
    {
        if (!potatoThrown)
        {
            if (!bobbing) StartCoroutine(BobUpAndDown());
            bobbing = true;
            xShift = xOffset / 10;
            yShift = yOffset / 10;
            StartCoroutine(FollowPlayer());
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
            StopCoroutine(FollowPlayer());
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

    private IEnumerator FollowPlayer()
    {
        Vector2 oldPlayerPosition = transform.position;
        yield return new WaitForSeconds(0.05f);
        // Set position to just behind the player, incorporating the bobbing and smooth return if needed
        if (initYOffset != 0)
        {
            potato.transform.position = oldPlayerPosition + new Vector2(xOffset - xShift, yOffset + 0.5f * ((initYOffset - yOffset) / initYOffset) + 0.5f) + bobOffset;
        }
        else
        {
            potato.transform.position = oldPlayerPosition + new Vector2(xOffset - xShift, yOffset + 1f) + bobOffset;
        }
        xOffset -= xShift;
        yOffset -= yShift;
        if (initYOffset - yOffset <= 0.01) initYOffset = 0;
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

            yield return new WaitForSeconds(0.02f);
        }
        rb.linearVelocity = Vector2.zero;

        // Set up offsets to manually return potato into position once it's close (for smoothness)
        xOffset = potato.transform.position.x - transform.position.x;
        yOffset = potato.transform.position.y - transform.position.y;
        initYOffset = yOffset;
        potatoThrown = false;

        playerSource.clip = catchSound;
        playerSource.Play();
        yield break;
    }

    private IEnumerator SmoothReturn()
    {
        // Set up offsets so the potato returns smoothly upon starting to follow the player again
        xOffset = potato.transform.position.x - transform.position.x;
        yOffset = potato.transform.position.y - transform.position.y;
        initYOffset = yOffset;
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
