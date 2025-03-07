using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random=UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float timeToExplode = 10f;
    List<GameObject> players = new List<GameObject>();
    bool exploded = false;

    void Start()
    {
        // Add each player in game to list
        foreach (Transform child in transform) {
            if (child.CompareTag("Player")) 
                players.Add(child.gameObject);
        }

        // Give a random player a potato
        ChoosePlayerToGivePotato();
    }

    void FixedUpdate()
    {
        if (!exploded) {
            // Potato explodes on 0
            if (timeToExplode <= 0f) {
                timer.text = "0";
                Explode();
            // Update timer otherwise
            } else {
                timer.text = timeToExplode.ToString();
                timeToExplode -= Time.fixedDeltaTime;
            }
        }
    }

    void ChoosePlayerToGivePotato()
    {
        int num = Random.Range(0, players.Count);
        Debug.Log("Random number between 0 and " + (players.Count - 1) + " is");
        Debug.Log(num);
        players[num].GetComponent<PlayerPotato>().getPotato.Invoke();
    }

    void Explode()
    {
        // Explode, killing player with the potato
        for (int i = 0; i < players.Count; ++i) {
            if (players[i].transform.GetComponent<PlayerPotato>().Potato().activeSelf) Destroy(players[i]);
        }

        exploded = true;
        Debug.Log("Potato exploded");
    }
}
