using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPotato : MonoBehaviour
{
    [SerializeField] float detectRadius;
    [SerializeField] PlayerVals player;

    public UnityEvent getPotato;
    [SerializeField] UnityEvent givePotato;

    [SerializeField] PlayerPotato enemy;

    bool playerFound = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void onAttack(InputValue val)
    {
        if (!playerFound) return;

        givePotato.Invoke();
        enemy.getPotato.Invoke();

    }

    public void onGetPotato()
    {
        player.setHasPotato(true);
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
            p = pot;
        }
    }
}
