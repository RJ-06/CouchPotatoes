using UnityEngine;

public class PlayerVals : MonoBehaviour
{
    //start game with these values
    [SerializeField] float baseMoveSpeed;
    [SerializeField] int baseHealthPoints;
    [SerializeField] float baseDashSpeed;
    [SerializeField] int baseAttackPoints;
    [SerializeField] float baseDashCooldown;
    [SerializeField] float baseAttackCooldown;
    [SerializeField] float baseMaxSpeed;

    //start game with these values
    float currentMoveSpeed;
    int currentHealthPoints;
    float currentDashSpeed;
    int currentAttackPoints;
    float currentDashCooldown;
    float currentAttackCooldown;
    float currentMaxSpeed;
    bool hasPotato = false;

    private void Start()
    {
        currentMoveSpeed = baseMoveSpeed;
        currentHealthPoints = baseHealthPoints;
        currentDashSpeed = baseDashSpeed;
        currentAttackPoints = baseAttackPoints;
        currentDashCooldown = baseDashCooldown;
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
    public float getAttackCooldown() { return currentAttackCooldown; }

    public void setAttackCooldown(int newAttackCooldown) { currentAttackCooldown = newAttackCooldown; }

    public float getMaxSpeed() { return currentMaxSpeed; }
    public void setMaxSpeed(float newMaxSpeed) { currentMaxSpeed = newMaxSpeed; }

}
