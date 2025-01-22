using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    ArrayList playerList = new ArrayList();
    GameObject[] players;

    [SerializeField] int playerCount;
    PlayerInputManager playerInputManager;

    int chosenPlayer = 0;

    bool joined = false;

    void Start()
    {
        /*for (int i = 0; i < playerCount; i++) { playerInputManager.JoinPlayer(); }
        var objects = FindObjectsByType<PlayerVals>(FindObjectsSortMode.None);
        foreach (var p in objects) { playerList.Add(p.gameObject); }
        chosenPlayer = Random.Range(0, playerList.Count);

        PlayerVals player = ((GameObject)playerList[chosenPlayer]).GetComponent<PlayerVals>();
        player.setHasPotato(true);*/

    }
}
