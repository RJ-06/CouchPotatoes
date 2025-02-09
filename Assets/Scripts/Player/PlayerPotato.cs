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
    [SerializeField] GameObject potato;
    [SerializeField] Rigidbody2D rb;

    [Tooltip("add in events")]
    public UnityEvent getPotato;
    [SerializeField] UnityEvent givePotato;

    [SerializeField] PlayerPotato enemy;

    bool playerFound = false;
    bool potatoThrown = false;


    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent.name == "Player") getPotato.Invoke();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!potatoThrown) rb.linearVelocity = transform.parent.GetComponent<Rigidbody2D>().linearVelocity;
    }

    private void OnAttack()
    {
        Debug.Log("Potato thrown");
        potatoThrown = true;
        rb.AddForce(new Vector2(200f, 200f));

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.getHasPotato()) return;

        if (playerFound = collision.TryGetComponent<PlayerPotato>(out PlayerPotato pot))
        {
            // p = pot;
        }
    }
}
