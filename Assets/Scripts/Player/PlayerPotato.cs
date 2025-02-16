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

    [Tooltip("add in events")]
    public UnityEvent getPotato;
    [SerializeField] UnityEvent givePotato;

    [SerializeField] PlayerPotato enemy;
    Vector2 bobOffset;
    bool playerFound = false;
    bool bobbing = false;
    bool potatoThrown = false;


    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent.name == "Player") getPotato.Invoke();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!potatoThrown)
        {
            if(!bobbing) StartCoroutine(BobUpAndDown());
            bobbing = true;
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
        potato.transform.position = oldPlayerPosition + new Vector2(0, 0.4f) + bobOffset;
    }

    private IEnumerator ReturnToPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        float mult = 50f;
        while(potato.transform.position.x - transform.position.x >= 0.1 || potato.transform.position.y - transform.position.y >= 0.1)
        {
            yield return new WaitForSeconds(0.1f);
            Vector2 returnForce = mult * new Vector2(transform.position.x - potato.transform.position.x, transform.position.y - potato.transform.position.y).normalized;
            rb.AddForce(returnForce);
            Debug.Log(rb.linearVelocity);
        }
        rb.linearVelocity = new Vector2(0,0);
        potatoThrown = false;
        yield break;
    }

    private void OnAttack()
    {
        Debug.Log("Potato thrown");
        StopCoroutine(FollowPlayer());
        rb.AddForce(500 * movement.lastMoveDir);
        StartCoroutine(ReturnToPlayer());
        potatoThrown = true;

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

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.getHasPotato()) return;

        if (playerFound = collision.TryGetComponent<PlayerPotato>(out PlayerPotato pot))
        {
            // p = pot;
        }
    }*/
}
