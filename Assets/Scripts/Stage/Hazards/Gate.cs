using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gate : MonoBehaviour
{
    [SerializeField] TilemapCollider2D gatePreventWalkOff, gateFallable;
    private Dictionary<Vector3Int, TileBase> disabledWalkoffTiles = new Dictionary<Vector3Int, TileBase>();
    private Dictionary<Vector3Int, TileBase> disabledFallableTiles = new Dictionary<Vector3Int, TileBase>();
    private List<GameObject> players = new List<GameObject>();
    private bool gateOpen = false;
    int exitIncrement = 0;

    // IMPORTANT: Since tilemap rigidbodies are typically static, they won't collide with each other by default. For gating functionality to
    // work, one of the tilemaps in the collision MUST have their rigidbody set to kinematic!!! For purposes of convention, add the kinematic
    // rigidbodies to gate tilemaps on moving platforms.

    void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log("Collision detected from a gate");
        players = FindAnyObjectByType<GameManager>().GetPlayers();
        if (collision.gameObject.CompareTag("Moving Platform Gate") || collision.gameObject.CompareTag("Land Gate"))
        {
            gateOpen = true;
            foreach (GameObject player in players)
            {
                if (player.GetComponent<PlayerMovement>().GetOnLandGate() && player.GetComponent<PlayerMovement>().GetOnMovingGate())
                {
                    player.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (GameObject player in FindAnyObjectByType<GameManager>().GetPlayers())
            {
                if (player.GetComponent<PlayerMovement>().GetOnLandGate() && player.GetComponent<PlayerMovement>().GetOnMovingGate())
                {
                    player.GetComponentInChildren<PlayerGateCheck>().SetInGate(true);
                    player.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Moving Platform Gate") || collision.gameObject.CompareTag("Land Gate"))
        {
            gateOpen = false;
        }

        if (exitIncrement >= 1 && collision.gameObject.transform.parent != null)
        {
            if (collision.gameObject.transform.parent.CompareTag("Player") && gameObject.CompareTag("Land Gate"))
            {
                collision.gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>().SetOnLandGate(false);
            }
            if (collision.gameObject.transform.parent.CompareTag("Player") && gameObject.CompareTag("Moving Platform Gate"))
            {
                collision.gameObject.transform.parent.gameObject.GetComponent<PlayerMovement>().SetOnMovingGate(false);
            }
            exitIncrement = 0;
        }
        else ++exitIncrement;
    }
}
