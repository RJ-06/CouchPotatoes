using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float timeToExplode = 10f;
    List<GameObject> players = new List<GameObject>();
    bool exploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in transform) {
            if (child.CompareTag("Player")) 
                players.Add(child.gameObject);
        }

        for (int i = 0; i < players.Count; ++i) {
            Debug.Log(players[i]);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!exploded) {
            if (timeToExplode <= 0f) {
                timer.text = "0";
                Explode();
            } else {
                timer.text = timeToExplode.ToString();
                timeToExplode -= Time.fixedDeltaTime;
            }
        }
    }

    void Explode()
    {
        for (int i = 0; i < players.Count; ++i) {
            if (players[i].transform.GetComponent<PlayerPotato>().Potato().activeSelf) Destroy(players[i]);
        }

        exploded = true;
        Debug.Log("Potato exploded");
    }
}
