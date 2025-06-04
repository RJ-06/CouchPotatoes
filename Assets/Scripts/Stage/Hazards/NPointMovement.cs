using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TwoPointMovement : MonoBehaviour
{
    [SerializeField] float moveTime, idleTime;
    [SerializeField] List<Vector2> movementPoints;
    [SerializeField] TilemapCollider2D colliderBounds;
    private List<PlayerMovement> players;
    private Vector2 previousPos, currentVelocity;
    private bool initialized = false, playerInside = false;

    void Start()
    {
        previousPos = transform.position;

        players = new List<PlayerMovement>();
        // Check for players inside the platform on scene load (might not be necessary)
        List<GameObject> allPlayers = FindAnyObjectByType<GameManager>().GetPlayers();
        foreach (GameObject player in allPlayers)
        {
            if (PlayerInsidePlatform(player) && !players.Contains(player.GetComponent<PlayerMovement>()))
            {
                players.Add(player.GetComponent<PlayerMovement>());
                playerInside = true;
                Debug.Log("A player is initially inside a platform");
            }
        }

    }

    void Update()
    {
        // Check for players inside the platform as first game starts
        if (!initialized && FindAnyObjectByType<GameManager>().GetFirstGameStarted())
        {
            List<GameObject> allPlayers = FindAnyObjectByType<GameManager>().GetPlayers();
            foreach (GameObject player in allPlayers)
            {
                if (player != null && PlayerInsidePlatform(player)
                && !players.Contains(player.GetComponent<PlayerMovement>()))
                {
                    players.Add(player.GetComponent<PlayerMovement>());
                    Debug.Log("A player is initially inside a platform");
                }
            }

            StartCoroutine(Move(moveTime, idleTime));
            initialized = true;
        }
    }

    void LateUpdate()
    {
        currentVelocity = (((Vector2)transform.position - previousPos) / Time.deltaTime) * 1.427f;
        previousPos = transform.position;
    }

    private IEnumerator Move(float timeToMove, float timeToIdle)
    {
        Vector2 previousPos;
        while (true)
        {
            float moveStartTime = Time.time, t = 0;
            for (int i = 1; i < movementPoints.Count; ++i)
            {
                // Move platform to second point
                yield return new WaitForSeconds(timeToIdle);
                moveStartTime = Time.time;
                t = 0;
                previousPos = new Vector2(transform.position.x, transform.position.y);
                while (t < 1)
                {
                    t = (Time.time - moveStartTime) / timeToMove;
                    transform.position = new Vector2(Mathf.SmoothStep(previousPos.x, movementPoints[i].x, t),
                    Mathf.SmoothStep(previousPos.y, movementPoints[i].y, t));
                    Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);

                    foreach (PlayerMovement player in players)
                    {
                        if (PlayerInsidePlatform(player.gameObject))
                        {
                            // Move the player relative to the platform's movement (no sliding)
                            player.SetOffsetVelocity(currentVelocity);
                        }
                    }
                    yield return null;
                }
            }

            // Move platform back to first point
            yield return new WaitForSeconds(timeToIdle);
            moveStartTime = Time.time;
            t = 0;
            previousPos = new Vector2(transform.position.x, transform.position.y);
            while (t < 1)
            {
                t = (Time.time - moveStartTime) / timeToMove;
                transform.position = new Vector2(Mathf.SmoothStep(previousPos.x, movementPoints[0].x, t),
                Mathf.SmoothStep(previousPos.y, movementPoints[0].y, t));
                Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);

                foreach (PlayerMovement player in players)
                {
                    if (PlayerInsidePlatform(player.gameObject))
                    {
                        // Move the player relative to the platform's movement (no sliding)
                        player.SetOffsetVelocity(currentVelocity);
                    }
                }
                yield return null;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Something is inside the moving platform");
        if (other.CompareTag("Player"))
        {
            Debug.Log("That something is a player");
            if (!players.Contains(other.gameObject.GetComponent<PlayerMovement>()))
            {
                players.Add(other.gameObject.GetComponent<PlayerMovement>());
                other.gameObject.GetComponent<PlayerMovement>().SetInsidePlatform(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerLeaving = players.Find(player => player == other.gameObject);
            players.Remove(players.Find(player => player == other.gameObject));
            playerLeaving.SetOffsetVelocity(Vector2.zero);
            playerLeaving.SetInsidePlatform(false);
        }
    }

    public bool PlayerInsidePlatform(GameObject player)
    {
        // Get dimensions of player
        float width = player.GetComponent<SpriteRenderer>().bounds.size.x;
        float height = player.GetComponent<SpriteRenderer>().bounds.size.y;

        // Check boundaries of player in comparison to the platform
        bool containsTop = colliderBounds.bounds.Contains(new Vector3(player.transform.position.x,
        player.transform.position.y + (height / 2), 0));
        bool containsBottom = colliderBounds.bounds.Contains(new Vector3(player.transform.position.x,
        player.transform.position.y - (height / 2), 0));
        bool containsLeft = colliderBounds.bounds.Contains(new Vector3(player.transform.position.x - (width / 2),
        player.transform.position.y, 0));
        bool containsRight = colliderBounds.bounds.Contains(new Vector3(player.transform.position.x + (width / 2),
        player.transform.position.y, 0));
        if (containsTop && containsBottom && containsLeft && containsRight)
        {
            Debug.Log("A player is fully inside a moving platform");
            player.GetComponent<PlayerMovement>().SetInsidePlatform(true);
            return true;
        }

        player.GetComponent<PlayerMovement>().SetInsidePlatform(false);
        return false;
    }
}
