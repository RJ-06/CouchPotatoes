using UnityEngine;

public class PlayerVals : MonoBehaviour
{
    //start game with these values
    float baseMoveSpeed;
    int baseHealthPoints;
    float baseDashSpeed;
    int baseAttackPoints;

    //start game with these values
    float currentMoveSpeed;
    int currentHealthPoints;
    float currentDashSpeed;
    int currentAttackPoints;
    bool hasPotato = false;

    private void Start()
    {
        float currentMoveSpeed = baseMoveSpeed;
        int currentHealthPoints = baseHealthPoints;
        float currentDashSpeed = baseDashSpeed;
        int currentAttackPoints = baseAttackPoints;
    }

    public float getMoveSpeed() { return currentMoveSpeed; }
    public void setMoveSpeed(float nSpeed) { currentMoveSpeed = nSpeed; }
    public float getHealth() { return currentHealthPoints; }
    public void setHealth(int nHealth) { currentHealthPoints = nHealth; }
    public void IncrementHealth(int healthChange) { currentHealthPoints += healthChange; }
    public float getDashSpeed() { return currentDashSpeed; }
    public void setDashSpeed(float nSpeed) { currentDashSpeed = nSpeed; }
    public float getAttackPoints() { return currentAttackPoints;}
    public void setAttackPoints(int nAttackPoints) { currentAttackPoints = nAttackPoints; }


}
