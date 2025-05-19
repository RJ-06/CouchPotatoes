using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TwoPointMovement : MonoBehaviour
{
    [SerializeField] float moveTime, idleTime;
    [SerializeField] Vector2 firstPoint, secondPoint;
    [SerializeField] TilemapCollider2D colliderBounds;
    private List<PlayerMovement> players;
    private bool initialized = false;

    void Start()
    {
        players = new List<PlayerMovement>();
        // Check for players inside the platform on scene load (might not be necessary)
        List<GameObject> allPlayers = transform.parent.gameObject.GetComponent<GameManager>().GetPlayers();
        foreach (GameObject player in allPlayers)
        {
            if (PlayerInsidePlatform(player) && !players.Contains(player.GetComponent<PlayerMovement>()))
            {
                players.Add(player.GetComponent<PlayerMovement>());
                Debug.Log("A player is initially inside a platform");
            }
        }

    }

    void Update()
    {
        // Check for players inside the platform as first game starts
        if (!initialized && transform.parent.gameObject.GetComponent<GameManager>().GetFirstGameStarted())
        {
            List<GameObject> allPlayers = transform.parent.gameObject.GetComponent<GameManager>().GetPlayers();
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

    private IEnumerator Move(float timeToMove, float timeToIdle)
    {
        while (true)
        {
            // Move platform to second point
            yield return new WaitForSeconds(timeToIdle);
            float moveStartTime = Time.time, t = 0;
            while (t < 1)
            {
                t = (Time.time - moveStartTime) / timeToMove;
                Vector2 previousPos = new Vector2(transform.position.x, transform.position.y);
                transform.position = new Vector2(Mathf.SmoothStep(firstPoint.x, secondPoint.x, t),
                Mathf.SmoothStep(firstPoint.y, secondPoint.y, t));
                Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);

                foreach (PlayerMovement player in players)
                {
                    if (PlayerInsidePlatform(player.gameObject))
                    {
                        // Move the player relative to the platform's movement (no sliding)
                        float newPosX = player.gameObject.transform.position.x + (currentPos.x - previousPos.x);
                        float newPosY = player.gameObject.transform.position.y + (currentPos.y - previousPos.y);
                        player.gameObject.transform.position = new Vector2(newPosX, newPosY);
                    }
                }
                yield return null;
            }

            // Move platform back to first point
            yield return new WaitForSeconds(timeToIdle);
            moveStartTime = Time.time;
            t = 0;
            while (t < 1)
            {
                t = (Time.time - moveStartTime) / timeToMove;
                Vector2 previousPos = new Vector2(transform.position.x, transform.position.y);
                transform.position = new Vector2(Mathf.SmoothStep(secondPoint.x, firstPoint.x, t),
                Mathf.SmoothStep(secondPoint.y, firstPoint.y, t));
                Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);

                foreach (PlayerMovement player in players)
                {
                    if (PlayerInsidePlatform(player.gameObject))
                    {
                        // Move the player relative to the platform's movement (no sliding)
                        float newPosX = player.gameObject.transform.position.x + (currentPos.x - previousPos.x);
                        float newPosY = player.gameObject.transform.position.y + (currentPos.y - previousPos.y);
                        player.gameObject.transform.position = new Vector2(newPosX, newPosY);
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
                players.Add(other.gameObject.GetComponent<PlayerMovement>());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            players.Remove(players.Find(player => player == other.gameObject));
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
            return true;
        }

        return false;
    }
}
