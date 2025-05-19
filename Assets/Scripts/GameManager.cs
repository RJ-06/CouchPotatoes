using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////

    // Game Components
    private SpriteRenderer potatoSprite;
    [SerializeField] Sprite happyPotato, expressionlessPotato, redPotato, veryRedPotato;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] List<Vector2> spawnPoints;
    [SerializeField] Tilemap possibleFallPoints;
    [SerializeField] Tilemap possibleRespawnPoints;
    private PlayerInputManager pInputManager;
    public List<GameObject> players = new List<GameObject>();
    private GameObject currentPlayer;
    [SerializeField] GameObject[] itemsToSpawn;

    // Related to gameplay
    [SerializeField] float minTimeToExplode = 10f;
    [SerializeField] float maxTimeToExplode = 35f;
    private float timeToExplode;
    public float time;
    private int numItems = 0, playerNum = 1, numOfPlayers, playersLeft;
    private bool firstGameStarted = false;
    private bool exploded = true;
    private bool playerNamesAssigned = false;

    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip mainBackground;
    public AudioClip testSFX;
    private bool mainMusicStarted = false;


    /////////////////////////////////
    ////////// STAGE INFO ///////////
    /////////////////////////////////

    // Floating grass island
    [SerializeField] public List<Vector2> floatingGrassIslandRespawnPoints = new List<Vector2>();

    void Start()
    {
        pInputManager = GetComponent<PlayerInputManager>();
        // Add each player in game to list
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                players.Add(child.gameObject);
            }
        }

        timer.text = "Press tab to start";
    }

    void FixedUpdate()
    {
        // Update potato sprite if potato transfers
        if (!exploded && PlayerWithPotato() != currentPlayer)
        {
            currentPlayer = PlayerWithPotato();
            potatoSprite = PlayerWithPotato().GetComponent<PlayerPotato>().Potato().GetComponent<SpriteRenderer>();
        }

        if (!exploded)
        {
            // Potato explodes on 0
            if (time <= 0f)
            {
                timer.text = "0";
                StartCoroutine(Explode());
                // Update timer otherwise
            }
            else
            {
                timer.text = time.ToString();
                time -= Time.fixedDeltaTime;
            }

            // Change sprite at halfway point
            if (time / timeToExplode <= 0.1f)
            {
                potatoSprite.sprite = veryRedPotato;
            }
            else if (time / timeToExplode <= 0.333f)
            {
                potatoSprite.sprite = redPotato;
            }
            else if (time / timeToExplode <= 0.5f)
            {
                potatoSprite.sprite = expressionlessPotato;
            }
            else potatoSprite.sprite = happyPotato;

            foreach (var p in players)
            {
                if (p.GetComponent<PlayerVals>())
                {
                    if (p.GetComponent<PlayerVals>().getHealth() <= 0) { KillPlayer(p); }

                }
            }
        }
    }

    //////////////////////////////////////
    ////////// GAME MANAGEMENT ///////////
    //////////////////////////////////////

    public void StartGame()
    {
        // Assign player names
        if (!playerNamesAssigned)
        {
            foreach (GameObject player in players)
            {
                player.name = "Player " + playerNum;
                ++playerNum;
                playerNamesAssigned = true;

                numOfPlayers = playerNum - 1;
                playersLeft = numOfPlayers;
            }
        }

        StartCoroutine(GameCountdown());
    }

    void ExecuteGame()
    {
        pInputManager.DisableJoining();
        if (!mainMusicStarted)
        {
            musicSource.clip = mainBackground;
            musicSource.Play();
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

    private IEnumerator GameCountdown()
    {
        timer.text = "Avoid the potato!";
        yield return new WaitForSeconds(3f);
        timer.text = "Ready...";
        yield return new WaitForSeconds(1f);
        timer.text = "Set...";
        yield return new WaitForSeconds(1f);
        timer.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        firstGameStarted = true;
        ExecuteGame();
    }

    void ChoosePlayerToGivePotato()
    {
        int num = Random.Range(0, playersLeft);
        while (!players[num].GetComponent<SpriteRenderer>().enabled)
        { // Ignore players that aren't alive
            int increment = Random.Range(0, 1);
            if (increment == 0) ++num;
            else --num;
        }
        players[num].GetComponent<PlayerPotato>().getPotato.Invoke();
    }

    GameObject PlayerWithPotato()
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i].transform.GetComponent<PlayerPotato>().Potato().activeSelf) return players[i];
        }
        Debug.Log("Player with potato was not found!");
        return null;
    }

    private IEnumerator PlaceItemsAtIntervals(float timeBetween)
    {
        while (numItems < players.Count * 4)
        {
            yield return new WaitForSeconds(timeBetween);
            ChooseWhereToPlaceItem();
        }
        yield return null;
    }

    void ChooseWhereToPlaceItem()
    {
        float[,] positions = {{0f, -21f, -21f, 21f, 21f},
                              {0f,   8f,  -8f,  8f, -8f}};
        int index = Random.Range(0, 5);

        // Place an item at the area with the largest distance sum
        Vector3 position = new Vector3(positions[0, index], positions[1, index], -0.5f);

        Instantiate(itemsToSpawn[Random.Range(0, itemsToSpawn.Length)], position, Quaternion.identity);
        ++numItems;
    }

    private void KillPlayer(GameObject player)
    {
        // player.SetActive(false);
        // ADD NEW CODE
        DeactivatePlayer(player);

        --playersLeft;
        if (player == PlayerWithPotato())
        {
            PlayerWithPotato().GetComponent<PlayerPotato>().ExplodePotato();
            PlayerWithPotato().GetComponent<PlayerPotato>().onGivePotato();
            StartCoroutine(BetweenPotatoExplosions());
        }

        if (playersLeft == 0) { RestoreAllPlayers(); }

    }

    private void DeactivatePlayer(GameObject player)
    {
        player.GetComponent<SpriteRenderer>().enabled = false;
        SpriteRenderer[] children = player.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer child in children)
        {
            child.enabled = false;
        }
        player.GetComponent<Collider2D>().enabled = false;
        player.GetComponent<PlayerMovement>().SetCanMove(false);
    }

    private void ReactivatePlayer(GameObject player)
    {
        player.GetComponent<SpriteRenderer>().enabled = true;
        SpriteRenderer[] children = player.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer child in children)
        {
            child.enabled = true;
        }
        player.GetComponent<Collider2D>().enabled = true;
        player.GetComponent<PlayerMovement>().SetCanMove(true);
    }

    private IEnumerator Explode()
    {
        // Explode, killing player with the potato
        while (PlayerWithPotato().GetComponent<PlayerMovement>().GetFallInProgress()) yield return new WaitForSeconds(0.1f);
        //PlayerWithPotato().GetComponent<PlayerPotato>().ExplodePotato();
        //PlayerWithPotato().SetActive(false);
        //PlayerWithPotato().GetComponent<PlayerPotato>().onGivePotato();

        exploded = true;
        //--playersLeft;
        //StartCoroutine(BetweenPotatoExplosions());

        KillPlayer(PlayerWithPotato());

        foreach (GameObject p in players)
        {
            if (p.activeSelf) p.GetComponent<PlayerVals>().setHealth(100);
        }

        yield return null;
    }

    private IEnumerator BetweenPotatoExplosions()
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (!players[i].activeSelf)
            {
                timer.text = players[i].name + " exploded!";
            }
        }
        yield return new WaitForSeconds(2f);

        // Handle win condition (if only one player is alive)
        if (playersLeft == 1)
        {
            foreach (GameObject player in players)
            {
                if (player.activeSelf) timer.text = player.name + " won!";
            }
            yield return new WaitForSeconds(3f);
            timer.text = "Respawning players...";
            RestoreAllPlayers();
            playersLeft = numOfPlayers;
        }
        yield return new WaitForSeconds(2f);
        StartGame();
    }

    void RestoreAllPlayers()
    {
        for (int i = 0; i < players.Count; ++i)
        {
            if (!players[i].GetComponent<SpriteRenderer>().enabled)
            {
                ReactivatePlayer(players[i]);
                players[i].GetComponent<PlayerVals>().setHealth(100);
                players[i].transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    public void IncrementTime(float timeIncr) => time += timeIncr;

    // public List<Vector2> getRespawnPoints() => floatingGrassIslandRespawnPoints;
    public Tilemap GetFallPoints() => possibleFallPoints;
    public Tilemap GetRespawnPoints() => possibleRespawnPoints;


    // Play Sound Effect. This will be called in other files
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public List<Vector2> GetSpawnPoints() => spawnPoints;

    public List<GameObject> GetPlayers() => players;

    public bool GetFirstGameStarted() => firstGameStarted;
}
