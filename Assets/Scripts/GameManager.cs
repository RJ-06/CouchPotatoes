using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
    [SerializeField] List<UnityEngine.Vector2> spawnPoints;
    [SerializeField] Tilemap possibleFallPoints;
    [SerializeField] Tilemap possibleRespawnPoints;
    [SerializeField] Tilemap possibleItemSpawns;
    private List<UnityEngine.Vector2> itemSpawnPositions = new List<UnityEngine.Vector2>();
    private PlayerInputManager pInputManager;
    public List<GameObject> players = new List<GameObject>();
    private List<GameObject> deadPlayers = new List<GameObject>();
    private GameObject currentPlayer;
    [SerializeField] GameObject[] itemsToSpawn;

    // Related to gameplay
    [SerializeField] float minTimeToSpawnItem = 5f;
    [SerializeField] float maxTimeToSpawnItem = 15f;
    [SerializeField] float minTimeToExplode = 10f;
    [SerializeField] float maxTimeToExplode = 35f;
    private float timeToExplode;
    public float time;
    private int numItems = 0, playerNum = 0, numOfPlayers, playersLeft;
    private bool firstGameStarted = false;
    private bool exploded = true;
    private bool playerNamesAssigned = false;
    [SerializeField] GameObject explosionEffect;

    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [SerializeField] PauseMenu pauseScript;

    [Header("---------- Audio Clip ----------")]
    public AudioClip mainBackground;
    public AudioClip testSFX;
    private bool mainMusicStarted = false;


    /////////////////////////////////
    ////////// STAGE INFO ///////////
    /////////////////////////////////

    // Floating grass island
    [SerializeField] public List<UnityEngine.Vector2> floatingGrassIslandRespawnPoints = new List<UnityEngine.Vector2>();

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

        // Initialize item spawnpoints
        foreach (Vector3Int pos in possibleItemSpawns.cellBounds.allPositionsWithin)
        {
            TileBase tile = possibleItemSpawns.GetTile(pos);
            if (tile != null)
            {
                itemSpawnPositions.Add(new UnityEngine.Vector2(pos.x, pos.y));
            }
        }

        timer.text = "Press tab to start";
    }

    void FixedUpdate()
    {
        // Check for new players before game starts
        if (!firstGameStarted)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Player") && !players.Contains(child.gameObject))
                {
                    players.Add(child.gameObject);
                }
            }
        }

        // Update potato sprite if potato transfers
        if (!exploded && PlayerWithPotato() != currentPlayer)
        {
            currentPlayer = PlayerWithPotato();
            potatoSprite = PlayerWithPotato().GetComponent<PlayerPotato>().Potato().GetComponent<SpriteRenderer>();
            currentPlayer.GetComponent<PlayerPotato>().SetPotatoIndicator(true);
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
            else if (PlayerWithPotato() != null)
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

    void OnPlayerJoined()
    {
        ++playerNum;
    }

    public void StartGame()
    {
        // Make sure games can't start without multiple players
        if (players.Count < 2)
        {
            timer.text = "Can't start a game without multiple players!";
            return;
        }

        // Assign player names
        if (!playerNamesAssigned)
        {
            int i = 1;
            foreach (GameObject player in players)
            {
                player.name = "Player " + i;
                ++i;
                playerNamesAssigned = true;
            }
        }

        playersLeft = players.Count;
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
        StartCoroutine(PlaceItemsAtIntervals(Random.Range(minTimeToSpawnItem, maxTimeToSpawnItem)));
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
        int i = Random.Range(0, itemSpawnPositions.Count - 1);
        UnityEngine.Vector2 position = itemSpawnPositions[i];

        Instantiate(itemsToSpawn[Random.Range(0, itemsToSpawn.Length)], position, UnityEngine.Quaternion.identity);
        ++numItems;
    }

    public void KillPlayer(GameObject player)
    {
        deadPlayers.Add(player);
        players.Remove(player);

        players.Remove(player);
        DeactivatePlayer(player);

        --playersLeft;
        Debug.Log(playersLeft);
        if (player == PlayerWithPotato())
        {
            PlayerWithPotato().GetComponent<PlayerPotato>().ExplodePotato();
            PlayerWithPotato().GetComponent<PlayerPotato>().onGivePotato();
            StartCoroutine(BetweenPotatoExplosions());
        }
        else if (PlayerWithPotato() == null)
        {
            StartCoroutine(BetweenPotatoExplosions());
        }
    }

    private void DeactivatePlayer(GameObject player)
    {
        player.GetComponent<SpriteRenderer>().enabled = false;
        SpriteRenderer[] children = player.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer child in children)
        {
            child.enabled = false;
        }
        player.GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<CircleCollider2D>().enabled = false;
        player.GetComponent<PlayerMovement>().SetCanMove(false);
        player.GetComponent<PlayerItems>().SetCanAttack(false);
        player.GetComponent<Rigidbody2D>().linearVelocity = UnityEngine.Vector2.zero;
    }

    private void ReactivatePlayer(GameObject player)
    {
        player.GetComponent<PlayerPotato>().onGivePotato();
        player.GetComponent<SpriteRenderer>().enabled = true;
        SpriteRenderer[] children = player.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer child in children)
        {
            child.enabled = true;
        }
        player.GetComponent<Collider2D>().enabled = true;
        player.GetComponent<PlayerMovement>().SetCanMove(true);
        player.GetComponent<PlayerItems>().SetCanAttack(true);
    }

    private IEnumerator Explode()
    {
        // Explode, killing player with the potato
        while (PlayerWithPotato().GetComponent<PlayerMovement>().GetFallInProgress()) yield return new WaitForSeconds(0.1f);
        //PlayerWithPotato().GetComponent<PlayerPotato>().ExplodePotato();
        //PlayerWithPotato().SetActive(false);
        //PlayerWithPotato().GetComponent<PlayerPotato>().onGivePotato();
        Instantiate(explosionEffect, PlayerWithPotato().GetComponent<Transform>().position, UnityEngine.Quaternion.identity);

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
            playersLeft = playerNum;
        }
        yield return new WaitForSeconds(2f);
        StartGame();
    }

    void RestoreAllPlayers()
    {
        for (int i = 0; i < deadPlayers.Count; ++i)
        {
            ReactivatePlayer(deadPlayers[i]);
            deadPlayers[i].GetComponent<PlayerVals>().setHealth(100);
            deadPlayers[i].transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
            players.Add(deadPlayers[i]);
            deadPlayers.Remove(deadPlayers[i]);
            --i;
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

    public List<UnityEngine.Vector2> GetSpawnPoints() => spawnPoints;

    public List<GameObject> GetPlayers() => players;

    public bool GetFirstGameStarted() => firstGameStarted;
    
    public PauseMenu GetPauseScript() => pauseScript;
}
