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
    [SerializeField] List<GameObject> players;
    private bool initialized = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<GameObject> allPlayers = transform.parent.gameObject.GetComponent<GameManager>().GetPlayers();
        foreach (GameObject player in allPlayers)
        {
            if (PlayerInsidePlatform(player))
            {
                Debug.Log("A player is initially inside a platform");
            }
        }

    }

    void Update()
    {
        if (!initialized && transform.parent.gameObject.GetComponent<GameManager>().GetFirstGameStarted())
        {
            List<GameObject> allPlayers = transform.parent.gameObject.GetComponent<GameManager>().GetPlayers();
            foreach (GameObject player in allPlayers)
            {
                if (PlayerInsidePlatform(player))
                {
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
                transform.position = new Vector2(Mathf.SmoothStep(firstPoint.x, secondPoint.x, t), Mathf.SmoothStep(firstPoint.y, secondPoint.y, t));

                foreach (GameObject player in players)
                {
                    if (PlayerInsidePlatform(player))
                    {
                        // Move the player relative to the platform's movement (no sliding)
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
                transform.position = new Vector2(Mathf.SmoothStep(secondPoint.x, firstPoint.x, t), Mathf.SmoothStep(secondPoint.y, firstPoint.y, t));
                yield return null;
            }
        }
    }

    // THIS DOES NOT WORK
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Something is inside the moving platform");
        if (other.CompareTag("Player"))
        {
            Debug.Log("That something is a player");
            if (!players.Contains(other.gameObject))
                players.Add(other.gameObject);
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
        bool containsTop = colliderBounds.bounds.Contains(new Vector3(player.transform.position.x, player.transform.position.y + (height / 2), 0));
        bool containsBottom = colliderBounds.bounds.Contains(new Vector3(player.transform.position.x, player.transform.position.y - (height / 2), 0));
        bool containsLeft = colliderBounds.bounds.Contains(new Vector3(player.transform.position.x - (width / 2), player.transform.position.y, 0));
        bool containsRight = colliderBounds.bounds.Contains(new Vector3(player.transform.position.x + (width / 2), player.transform.position.y, 0));
        if (containsTop && containsBottom && containsLeft && containsRight)
        {
            Debug.Log("A player is fully inside a moving platform");
            return true;
        }

        return false;
    }
}
