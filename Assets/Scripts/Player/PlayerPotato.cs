using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPotato : MonoBehaviour
{
    [Header("put this on an empty with a collider attached that is then parented to the player")]

    [SerializeField] PlayerVals player;
    [SerializeField] PlayerMovement movement;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject potato;
    [SerializeField] CircleCollider2D potatoChecker;

    [Tooltip("add in events")]
    public UnityEvent getPotato;
    [SerializeField] UnityEvent givePotato;

    [SerializeField] PlayerPotato enemy;
    Vector2 bobOffset;
    float xOffset = 0f, yOffset = 0f, initYOffset, xShift, yShift;
    bool bobbing = false;
    bool potatoThrown = false;
    bool atPlayer = false;


    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name == "Player") getPotato.Invoke();
        potatoChecker.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!potatoThrown)
        {
            if(!bobbing) StartCoroutine(BobUpAndDown());
            bobbing = true;
            xShift = xOffset / 10;
            yShift = yOffset / 10;
            StartCoroutine(FollowPlayer());
        }
        else
        {
            float totalVelocity = Mathf.Sqrt(Mathf.Pow(rb.linearVelocityX, 2) + Mathf.Pow(rb.linearVelocityY, 2));
        }
    }

    private IEnumerator BobUpAndDown()
    {
        Vector2 corePos = potato.transform.position;
        while(true)
        {
            for(float i = 1f; i <= 100f; ++i)
            {
                bobOffset = new Vector2(0, 0.15f * Mathf.Sin(0.02f * Mathf.PI * i));
                if(potatoThrown)
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
        if(initYOffset != 0)
        {
            potato.transform.position = oldPlayerPosition + new Vector2(xOffset - xShift, yOffset + 0.5f * ((initYOffset - yOffset) / initYOffset)) + bobOffset;
        }
        else
        {
            potato.transform.position = oldPlayerPosition + new Vector2(xOffset - xShift, yOffset + 0.5f) + bobOffset;
        }
        xOffset -= xShift;
        yOffset -= yShift;
        if(initYOffset - yOffset <= 0.01) initYOffset = 0;
    }

    private IEnumerator ReturnToPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        atPlayer = false;
        float mult = 50f;

        // Make potato "gravitationally" return to player until it is reasonably close
        float exp = 0f;
        while(!atPlayer)
        {
            Vector2 returnForce = (float)Math.Pow(2, exp) * mult * new Vector2(transform.position.x - potato.transform.position.x, transform.position.y + 0.5f - potato.transform.position.y).normalized;
            rb.linearVelocity = 0.8f * rb.linearVelocity;
            rb.AddForce(returnForce);
            exp += 0.15f;

            // Stop coroutine if potato transfers
            if (!player.getHasPotato()) yield break;

            yield return new WaitForSeconds(0.02f);
        }
        rb.linearVelocity = new Vector2(0,0);

        // Set up offsets to manually return potato into position once it's close (for smoothness)
        xOffset = potato.transform.position.x - transform.position.x;
        yOffset = potato.transform.position.y - transform.position.y;
        initYOffset = yOffset;
        potatoThrown = false;
        yield break;
    }

    private IEnumerator SmoothReturn()
    {
        xOffset = potato.transform.position.x - transform.position.x;
        yOffset = potato.transform.position.y - transform.position.y;
        initYOffset = yOffset;
        yield break;
    }

    private void OnThrow()
    {
        if(!potatoThrown && player.getHasPotato())
        {
            StopCoroutine(FollowPlayer());
            rb.AddForce(750 * movement.lastMoveDir);
            StartCoroutine(ReturnToPlayer());
            Debug.Log("Return to player started for " + transform.gameObject);
            potatoThrown = true;
        }
    }

    public void onGetPotato()
    {
        player.setHasPotato(true);
        potato.SetActive(true);
    }

    public void onGivePotato()
    {
        potatoThrown = false;
        player.setHasPotato(false);
        potato.SetActive(false);
    }

    public bool getPotatoThrown() { return potatoThrown; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(potatoThrown && other.CompareTag("Potato") && other.transform.parent.gameObject == transform.gameObject)
        {
            atPlayer = true;
        }
        else if(!player.getHasPotato() && other.CompareTag("Potato") && other.transform.parent.GetComponent<PlayerPotato>().getPotatoThrown())
        {
            Debug.Log("Branch taken");
            PlayerPotato giver = other.transform.parent.GetComponent<PlayerPotato>();

            float transferX = giver.potato.transform.position.x;
            float transferY = giver.potato.transform.position.y;
            GetComponent<PlayerPotato>().potato.transform.position = new Vector2(transferX, transferY);
            giver.givePotato.Invoke();
            getPotato.Invoke();
            StartCoroutine(SmoothReturn());
        }
    }
}
