using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class DynamicCamera : MonoBehaviour
{
    CinemachineTargetGroup targetGroup;
    [SerializeField] GameManager gameManager;

    private void Start()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
        if (targetGroup == null)
        {
            Debug.LogError("CinemachineTargetGroup component not found on this GameObject.");
        }
    }

    private void Update()
    {
        // Fix for CS0029: Convert the array to a List before assigning it to targetGroup.Targets  
        targetGroup.Targets = new List<CinemachineTargetGroup.Target>(
            gameManager.players.ConvertAll(player => new CinemachineTargetGroup.Target
            {
                Object = player.transform,
                Weight = 1f,
                Radius = 0.5f
            })
        );
    }
}
