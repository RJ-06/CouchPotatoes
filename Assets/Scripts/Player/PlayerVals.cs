using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerVals : MonoBehaviour
{
    // Start game with these values
    [SerializeField] float baseMoveSpeed;
    [SerializeField] int baseHealthPoints;
    [SerializeField] float baseDashSpeed;
    [SerializeField] int baseAttackPoints;
    [SerializeField] float baseDashCooldown;
    [SerializeField] float baseDashTime;
    [SerializeField] float baseAttackCooldown;
    [SerializeField] float baseMaxSpeed;

    // Start game with these values
    float currentMoveSpeed;
    float movementMultiplier = 1f;
    float speedSensitivityMultiplier = 1f;
    int currentHealthPoints;
    float currentDashSpeed;
    int currentAttackPoints;
    float currentDashCooldown;
    float currentDashTime;
    float currentAttackCooldown;
    float currentMaxSpeed;
    public static int numPlayers = 0;
    public int playerNum = -1;
    public bool hasPotato = false;
    bool isFrozen;

    private void Awake()
    {
        ++numPlayers;
        playerNum = numPlayers;

        // Place player at a spawn point
        List<Vector2> spawnPoints = FindAnyObjectByType<GameManager>().GetSpawnPoints();
        transform.position = spawnPoints[(numPlayers - 1) % spawnPoints.Count];

        currentMoveSpeed = baseMoveSpeed;
        currentHealthPoints = baseHealthPoints;
        currentDashSpeed = baseDashSpeed;
        currentAttackPoints = baseAttackPoints;
        currentDashCooldown = baseDashCooldown;
        currentDashTime = baseDashTime;
        currentAttackCooldown = baseAttackCooldown;
        currentMaxSpeed = baseMaxSpeed;
    }

    public float getMoveSpeed() => currentMoveSpeed;
    public void setMoveSpeed(float nSpeed) => currentMoveSpeed = nSpeed;
    public float getMovementMultiplier() => movementMultiplier;
    public void setMovementMultiplier(float nMult) => movementMultiplier = nMult;
    public float getSpeedSensitivityMultiplier() => speedSensitivityMultiplier;
    public void setSpeedSensitivityMultiplier(float nMult) => speedSensitivityMultiplier = nMult;
    public float getHealth() => currentHealthPoints;
    public float getMaxHealth() => baseHealthPoints;
    public void setHealth(int nHealth) => currentHealthPoints = nHealth;
    public void IncrementHealth(int healthChange) => currentHealthPoints += healthChange;
    public float getDashSpeed() => currentDashSpeed;
    public void setDashSpeed(float nSpeed) => currentDashSpeed = nSpeed;
    public float getAttackPoints() => currentAttackPoints;
    public void setAttackPoints(int nAttackPoints) => currentAttackPoints = nAttackPoints;

    public float getDashCooldown() => currentDashCooldown;
    public void setDashCooldown(float newDashCooldown) => currentDashCooldown = newDashCooldown;
    public float getDashTime() => currentDashTime;
    public void setDashTime(float newDashTime) => currentDashTime = newDashTime;
    public float getAttackCooldown() => currentAttackCooldown;

    public void setAttackCooldown(float newAttackCooldown) => currentAttackCooldown = newAttackCooldown;

    public float getMaxSpeed() => currentMaxSpeed;
    public void setMaxSpeed(float newMaxSpeed) => currentMaxSpeed = newMaxSpeed;

    public bool getHasPotato() => hasPotato;
    public void setHasPotato(bool state) => hasPotato = state;

    public bool getFrozen() => isFrozen;
    public void setFrozen(bool state) => isFrozen = state;
}
