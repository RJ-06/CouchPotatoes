using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static int playerCount;
    private List<PlayerInput> players = new List<PlayerInput>();
    private PlayerInputManager playerInputManager;
    [SerializeField] Transform[] startingPositions;

    public static List<GameObject> playerList = new List<GameObject>(); // temporary

    private void Awake()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        for (int i = 0; i < playerCount; i++) 
        {
            playerInputManager.JoinPlayer();
        }
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);

        //need to use the parent due to the structure of the prefab
        Transform playerParent = player.transform.parent;
        playerParent.position = startingPositions[players.Count - 1].position;

    }

}
