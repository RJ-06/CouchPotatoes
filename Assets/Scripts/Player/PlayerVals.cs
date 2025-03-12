using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerVals : MonoBehaviour
{
    //start game with these values
    [SerializeField] float baseMoveSpeed;
    [SerializeField] int baseHealthPoints;
    [SerializeField] float baseDashSpeed;
    [SerializeField] int baseAttackPoints;
    [SerializeField] float baseDashCooldown;
    [SerializeField] float baseDashTime;
    [SerializeField] float baseAttackCooldown;
    [SerializeField] float baseMaxSpeed;

    //start game with these values
    float currentMoveSpeed;
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

    private void Awake()
    {
        ++numPlayers;
        playerNum = numPlayers;

        currentMoveSpeed = baseMoveSpeed;
        currentHealthPoints = baseHealthPoints;
        currentDashSpeed = baseDashSpeed;
        currentAttackPoints = baseAttackPoints;
        currentDashCooldown = baseDashCooldown;
        currentDashTime = baseDashTime;
        currentAttackCooldown = baseAttackCooldown;
        currentMaxSpeed = baseMaxSpeed;
    }

    public float getMoveSpeed() { return currentMoveSpeed; }
    public void setMoveSpeed(float nSpeed) { currentMoveSpeed = nSpeed; }
    public float getHealth() { return currentHealthPoints; }
    public void setHealth(int nHealth) { currentHealthPoints = nHealth; }
    public void IncrementHealth(int healthChange) { currentHealthPoints += healthChange; }
    public float getDashSpeed() { return currentDashSpeed; }
    public void setDashSpeed(float nSpeed) { currentDashSpeed = nSpeed; }
    public float getAttackPoints() { return currentAttackPoints; }
    public void setAttackPoints(int nAttackPoints) { currentAttackPoints = nAttackPoints; }

    public float getDashCooldown() { return currentDashCooldown; }
    public void setDashCooldown(int newDashCooldown) { currentDashCooldown = newDashCooldown; }
    public float getDashTime() { return currentDashTime; }
    public void setDashTime(int newDashTime) { currentDashTime = newDashTime; }
    public float getAttackCooldown() { return currentAttackCooldown; }

    public void setAttackCooldown(int newAttackCooldown) { currentAttackCooldown = newAttackCooldown; }

    public float getMaxSpeed() { return currentMaxSpeed; }
    public void setMaxSpeed(float newMaxSpeed) { currentMaxSpeed = newMaxSpeed; }

    public bool getHasPotato() { return hasPotato; }
    public void setHasPotato(bool state) { hasPotato = state; }


    
    // NOTES from Jason: This is a testing function of displaying the health bar
    // Feel free to try and delete it
    void Update()
    {
        // Test function - press T to take 1 damage
        if (Input.GetKeyDown(KeyCode.T))
        {
            IncrementHealth(-1);
            Debug.Log("Player took 1 damage. Current health: " + currentHealthPoints);
        }
        // Press H to heal 1 health
        if (Input.GetKeyDown(KeyCode.H))
        {
            IncrementHealth(1);
            Debug.Log("Player healed 1 health. Current health: " + currentHealthPoints);
        }
    }
}
