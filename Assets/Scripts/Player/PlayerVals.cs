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

    public float getMoveSpeed() => currentMoveSpeed;
    public void setMoveSpeed(float nSpeed) => currentMoveSpeed = nSpeed;
    public float getHealth() => currentHealthPoints;
    public float getMaxHealth() => baseHealthPoints;
    public void setHealth(int nHealth)  => currentHealthPoints = nHealth;
    public void IncrementHealth(int healthChange) => currentHealthPoints += healthChange;
    public float getDashSpeed() => currentDashSpeed;
    public void setDashSpeed(float nSpeed) => currentDashSpeed = nSpeed;
    public float getAttackPoints() => currentAttackPoints;
    public void setAttackPoints(int nAttackPoints) => currentAttackPoints = nAttackPoints;

    public float getDashCooldown() => currentDashCooldown;
    public void setDashCooldown(int newDashCooldown) => currentDashCooldown = newDashCooldown;
    public float getDashTime() => currentDashTime;
    public void setDashTime(int newDashTime) => currentDashTime = newDashTime;
    public float getAttackCooldown() => currentAttackCooldown;

    public void setAttackCooldown(int newAttackCooldown) => currentAttackCooldown = newAttackCooldown;

    public float getMaxSpeed() => currentMaxSpeed;
    public void setMaxSpeed(float newMaxSpeed) => currentMaxSpeed = newMaxSpeed;

    public bool getHasPotato() => hasPotato;
    public void setHasPotato(bool state) => hasPotato = state;
}
