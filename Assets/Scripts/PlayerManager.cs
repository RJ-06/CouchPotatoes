using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    ArrayList playerList = new ArrayList();

    void Start()
    {
        var objects = FindObjectsByType<PlayerVals>(FindObjectsSortMode.None);
        foreach (var p in objects) { playerList.Add(p.gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
