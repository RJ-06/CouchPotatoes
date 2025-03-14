using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random=UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    SpriteRenderer potatoSprite;
    [SerializeField] Sprite happyPotato, expressionlessPotato, redPotato, veryRedPotato;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float minTimeToExplode = 10f;
    [SerializeField] float maxTimeToExplode = 35f;
    float timeToExplode;

    [SerializeField] GameObject itemPrefab;
    [SerializeField] AudioSource audio;
    public float time;
    public List<GameObject> players = new List<GameObject>();
    GameObject currentPlayer;
    bool exploded = true;
    bool playerNamesAssigned = false;
    bool mainMusicStarted = false;
    int numItems = 0, playerNum = 1, numOfPlayers, playersLeft;

    PlayerInputManager pInputManager;

    //////////////////////////////////////////////////////////////////////////////////////
    // STAGE INFO ////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////

    // FLOATING GRASS ISLAND
    [SerializeField]
    public List<Vector2> floatingGrassIslandRespawnPoints = new List<Vector2>();

    void Start()
    {
        pInputManager = GetComponent<PlayerInputManager>();
        // Add each player in game to list
        foreach (Transform child in transform) {
            if (child.CompareTag("Player")) {
                players.Add(child.gameObject);
                Debug.Log("Player joined");
            }
        }

        timer.text = "Press tab to start";

        //StartGame();
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
                StartCoroutine(Explode());
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
            } else potatoSprite.sprite = happyPotato;
        }
    }

    void ChoosePlayerToGivePotato()
    {
        int num = Random.Range(0, playersLeft);
        while (!players[num].activeSelf) {
            int increment = Random.Range(0, 2);
            if (increment == 0) ++num;
            else --num;
        }
        players[num].GetComponent<PlayerPotato>().getPotato.Invoke();
    }

    public void StartGame()
    {
        if (!playerNamesAssigned) {
            foreach (GameObject player in players) {
                player.name = "Player " + playerNum;
                ++playerNum;
                playerNamesAssigned = true;

                numOfPlayers = playerNum - 1;
                playersLeft = numOfPlayers;
            }
        }

        StartCoroutine(GameCountdown());
        Debug.Log("Game started");
        
    }

    void ExecuteGame()
    {
        pInputManager.DisableJoining();
        if(!mainMusicStarted) {
            audio.Play();
            mainMusicStarted = true;
        }
        timeToExplode = Random.Range(minTimeToExplode, maxTimeToExplode);
        time = timeToExplode;
        exploded = false;

        // Give a random player a potato
        ChoosePlayerToGivePotato();

        currentPlayer = PlayerWithPotato();
        potatoSprite = PlayerWithPotato().GetComponent<PlayerPotato>().Potato().GetComponent<SpriteRenderer>();

        // Start item spawning
        StartCoroutine(PlaceItemsAtIntervals(15f));
    }

    private IEnumerator Explode()
    {
        // Explode, killing player with the potato
        while (PlayerWithPotato().GetComponent<PlayerMovement>().GetFallInProgress()) yield return new WaitForSeconds(0.1f);
        PlayerWithPotato().GetComponent<PlayerPotato>().ExplodePotato();
        PlayerWithPotato().SetActive(false);
        PlayerWithPotato().GetComponent<PlayerPotato>().onGivePotato();

        exploded = true;
        --playersLeft;
        StartCoroutine(BetweenPotatoExplosions());
        yield return null;
    }

    void RestoreAllPlayers()
    {
        for (int i = 0; i < players.Count; ++i) {
            if (!players[i].activeSelf) {
                players[i].SetActive(true);
                players[i].transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    void ChooseWhereToPlaceItem() {
        float[,] positions = {{0f, -21f, -21f, 21f, 21f},
                              {0f,   8f,  -8f,  8f, -8f}};
        float[] distanceSums = {0f, 0f, 0f, 0f, 0f};
        float largestDistSum = 0f;
        int index = -1;
        bool placeInCenter = true;

        // Loop through and find largest distance sum and its index
        for (int i = 0; i < distanceSums.Length; ++i) {
            for (int j = 0; j < players.Count; ++j) {
                float xDist = Mathf.Abs(players[j].transform.position.x - positions[0,i]);
                float yDist = Mathf.Abs(players[j].transform.position.y - positions[1,i]);
                float dist = Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2));

                distanceSums[i] += dist;
            }
            if (distanceSums[i] > largestDistSum) {
                largestDistSum = distanceSums[i];
                index = i;
            }
        }

        // Determine whether to place in center
        for (int i = 1; i < distanceSums.Length; ++i) {
            if (1.3 * distanceSums[0] < distanceSums[i]) placeInCenter = false;
        }

        // Place an item at the area with the largest distance sum
        Vector3 position = new Vector3(positions[0,index], positions[1,index], -0.5f);
        
        Instantiate(itemPrefab, position, Quaternion.identity);
        ++numItems;
    }

    GameObject PlayerWithPotato() {
        for (int i = 0; i < players.Count; ++i) {
            if (players[i].transform.GetComponent<PlayerPotato>().Potato().activeSelf) return players[i];
        }
        Debug.Log("Player with potato was not found!");
        return null;
    }

    private IEnumerator GameCountdown()
    {
        timer.text = "Avoid the potato!";
        Debug.Log("Text displayed");
        yield return new WaitForSeconds(3f);
        timer.text = "Ready...";
        yield return new WaitForSeconds(1f);
        timer.text = "Set...";
        yield return new WaitForSeconds(1f);
        timer.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        ExecuteGame();
    }

    private IEnumerator BetweenPotatoExplosions()
    {
        for (int i = 0; i < players.Count; ++i) {
            if (!players[i].activeSelf) {
                timer.text = players[i].name + " exploded!";
            }
        }
        yield return new WaitForSeconds(2f);
        
        if (playersLeft == 1) {
            foreach (GameObject player in players) {
                if(player.activeSelf) timer.text = player.name + " won!";
            }
            yield return new WaitForSeconds(3f);
            timer.text = "Respawning players...";
            RestoreAllPlayers();
            playersLeft = numOfPlayers;
        }
        yield return new WaitForSeconds(2f);
        StartGame();
    }

    private IEnumerator PlaceItemsAtIntervals(float timeBetween)
    {
        while (numItems < players.Count) {
            Debug.Log("Got past if statement");
            yield return new WaitForSeconds(timeBetween);
            ChooseWhereToPlaceItem();
        }
        yield return null;
    }

    public void IncrementTime(float timeIncr) 
    {
        time += timeIncr;
    }

    public List<Vector2> getRespawnPoints() => floatingGrassIslandRespawnPoints;
}
