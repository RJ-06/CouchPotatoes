using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random=UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    SpriteRenderer potatoSprite;
    [SerializeField] Sprite expressionlessPotato, redPotato, veryRedPotato;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float timeToExplode = 10f;
    float time;
    List<GameObject> players = new List<GameObject>();
    GameObject currentPlayer;
    bool exploded = false;

    void Start()
    {
        // Add each player in game to list
        foreach (Transform child in transform) {
            if (child.CompareTag("Player")) 
                players.Add(child.gameObject);
        }

        time = timeToExplode;

        // Give a random player a potato
        ChoosePlayerToGivePotato();

        currentPlayer = PlayerWithPotato();
        potatoSprite = PlayerWithPotato().GetComponent<PlayerPotato>().Potato().GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Update potato sprite if potato transfers
        if (!exploded && PlayerWithPotato() != currentPlayer) {
            currentPlayer = PlayerWithPotato();
            potatoSprite = PlayerWithPotato().GetComponent<PlayerPotato>().Potato().GetComponent<SpriteRenderer>();
        }

        if (!exploded) {
            // Potato explodes on 0
            if (time <= 0f) {
                timer.text = "0";
                Explode();
            // Update timer otherwise
            } else {
                timer.text = time.ToString();
                time -= Time.fixedDeltaTime;
            }

            // Change sprite at halfway point
            if (time / timeToExplode <= 0.1f) {
                potatoSprite.sprite = veryRedPotato;
            } else if (time / timeToExplode <= 0.333f) {
                potatoSprite.sprite = redPotato;
            } else if (time / timeToExplode <= 0.5f) {
                potatoSprite.sprite = expressionlessPotato;
            }
        }
    }

    void ChoosePlayerToGivePotato()
    {
        int num = Random.Range(0, players.Count);
        players[num].GetComponent<PlayerPotato>().getPotato.Invoke();
    }

    void Explode()
    {
        // Explode, killing player with the potato
        PlayerWithPotato().GetComponent<PlayerPotato>().ExplodePotato();
        Destroy(PlayerWithPotato());

        exploded = true;
        Debug.Log("Potato exploded");
    }

    GameObject PlayerWithPotato() {
        for (int i = 0; i < players.Count; ++i) {
            if (players[i].transform.GetComponent<PlayerPotato>().Potato().activeSelf) return players[i];
        }
        Debug.Log("Player with potato was not found!");
        return null;
    }
}
