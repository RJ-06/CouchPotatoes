using System;
using System.Collections;
using System.Collections.Generic;
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
    bool playerFound = false;
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

        //if(!potatoThrown) rb.linearVelocity = transform.parent.GetComponent<Rigidbody2D>().linearVelocity;
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
        if(initYOffset != 0)
        {
            Debug.Log(0.5f * ((initYOffset - yOffset) / initYOffset));
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
        while(!atPlayer)
        {
            Vector2 returnForce = mult * new Vector2(transform.position.x - potato.transform.position.x, transform.position.y + 0.5f - potato.transform.position.y).normalized;
            rb.AddForce(returnForce);
            yield return new WaitForSeconds(0.02f);
        }
        rb.linearVelocity = new Vector2(0,0);

        // After potato is close to player manually return it to its exact position for smoothness
        /*float xMove = (transform.position.x - potato.transform.position.x) / 10f;
        float yMove = (transform.position.y + 0.5f - potato.transform.position.y) / 10f;
        while(Mathf.Abs(transform.position.x - potato.transform.position.x) >= 0.05f || Mathf.Abs(transform.position.y - potato.transform.position.y) >= 0.05f)
        {
            yield return new WaitForSeconds(0.01f);
            potato.transform.position = new Vector2(potato.transform.position.x + xMove, potato.transform.position.y + yMove);
        }*/
        xOffset = potato.transform.position.x - transform.position.x;
        yOffset = potato.transform.position.y - transform.position.y;
        initYOffset = yOffset;
        potatoThrown = false;
        yield break;
    }

    private void OnAttack()
    {
        if(!potatoThrown)
        {
            StopCoroutine(FollowPlayer());
            rb.AddForce(500 * movement.lastMoveDir);
            StartCoroutine(ReturnToPlayer());
            potatoThrown = true;
        }

        /*if (!playerFound) return;

        givePotato.Invoke();
        enemy.getPotato.Invoke();*/

    }

    public void onGetPotato()
    {
        player.setHasPotato(true);
        potato.SetActive(true);
    }

    public void onGivePotato()
    {
        player.setHasPotato(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(potatoThrown && other.CompareTag("Potato") && other.transform.parent.gameObject == gameObject)
        {
            atPlayer = true;
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.getHasPotato()) return;

        if (playerFound = collision.TryGetComponent<PlayerPotato>(out PlayerPotato pot))
        {
            // p = pot;
        }
    }*/
}
