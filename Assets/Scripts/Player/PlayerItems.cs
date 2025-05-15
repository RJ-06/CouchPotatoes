using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems: MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    // Player components
    private PlayerVals pv;
    private PlayerMovement movement;
    private Transform shockwaveItem;
    [SerializeField] GameObject ShockwavePrefab;
    [SerializeField] GameObject weakAttackPrefab;
    [SerializeField] GameObject ConfusionItem;
    [SerializeField] GameObject frenzyPowerup;

    // Weak attack
    [SerializeField] float weakCooldown;
    [SerializeField] float weakCooldownTimer;
    private bool weakUsed = false;
    private Vector2 shootDir;

    // Shockwave attack
    [SerializeField] float shockwaveCooldown;
    [SerializeField] float shockwaveCooldownTimer;
    private bool shockwaveUsed = false;

    // Confusion item
    [SerializeField] float confusionDuration;
    protected float confusionTimer = 0;

    //frenzy item
    [SerializeField] float frenzyBoost;
    [SerializeField] float frenzyCooldownDecrease;


    void Start()
    {
        pv = GetComponent<PlayerVals>();
        movement = GetComponent<PlayerMovement>();
        shockwaveCooldown = pv.getAttackCooldown();
    }

    void FixedUpdate()
    {
        // Handle cooldowns and attack executions
        if (shockwaveUsed && Mathf.Abs(shockwaveCooldownTimer) >= 0.0001) {
            shockwaveCooldownTimer -= Time.fixedDeltaTime;
        } else if (shockwaveUsed) {
            shockwaveUsed = false;
        }

        if (weakUsed && Mathf.Abs(weakCooldownTimer) >= 0.00001) {
            weakCooldownTimer -= Time.fixedDeltaTime;
        } else if (weakUsed) {
            weakUsed = false;
        }

        Transform confIt = transform.Find("ConfusionItem(Clone)");
        if (confIt != null) 
        {
            transform.gameObject.GetComponent<Rigidbody2D>().linearVelocity *= -1f;
            pv.setMovementMultiplier(-1f);
            Debug.Log("movemult: " + pv.getMovementMultiplier() + "; movespeed: " + pv.getMoveSpeed());
            Destroy(confIt.gameObject);
            StartCoroutine("ConfusionTime");

        }
        confIt = transform.Find("ConfusionItem(Clone)");
        if (confIt != null)
        {
            pv.setMovementMultiplier(pv.getMovementMultiplier() * frenzyBoost);
            pv.setAttackCooldown(pv.getAttackCooldown() * frenzyCooldownDecrease);
            pv.setDashCooldown(pv.getDashCooldown() * frenzyCooldownDecrease);
            Destroy(confIt.gameObject);
        }
    }


    ////////////////////////////
    ////////// INPUTS //////////
    ////////////////////////////
    
    private void OnAttack()
    {
        shockwaveItem = transform.Find("ShockwaveItem(Clone)");

        if(shockwaveItem != null && !shockwaveUsed) // Shockwave attack used
        {
            shockwaveUsed = true;
            shockwaveCooldownTimer = shockwaveCooldown;
            Instantiate(ShockwavePrefab, transform.position, Quaternion.identity, this.transform);
        }
        else if(shockwaveItem == null && !weakUsed) // No other attack available, so do weak attack
        {
            weakCooldownTimer = weakCooldown;
            WeakAttack();
        }
    }


    /////////////////////////////////
    ////////// WEAK ATTACK //////////
    /////////////////////////////////
    
    private void WeakAttack()
    {
        if (shootDir != Vector2.zero){
            Vector2 normalizedShootDir = shootDir.normalized;
            Vector3 attackPos = new Vector3(
                transform.position.x + normalizedShootDir.x, 
                transform.position.y + normalizedShootDir.y, 
                transform.position.z 
                );        
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, shootDir);
            GameObject weakAttack = Instantiate(weakAttackPrefab, attackPos, rot, transform.gameObject.transform);

            Destroy(weakAttack, 1f);
        }
        else{
            Vector2 normalizedShootDir = movement.lastMoveDir.normalized;
            Vector3 attackPos = new Vector3(
                transform.position.x + normalizedShootDir.x, 
                transform.position.y + normalizedShootDir.y, 
                transform.position.z 
                );        
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, movement.lastMoveDir);
            GameObject weakAttack = Instantiate(weakAttackPrefab, attackPos, rot, transform.gameObject.transform);

            Destroy(weakAttack, 1f);
        }
        
    }

    private void OnAim(InputValue val) // Aim the potato with the right joystick on controller
    {
        shootDir = val.Get<Vector2>();
    }


    /////////////////////////////////
    /////// Confusion Duration //////
    /////////////////////////////////

    IEnumerator ConfusionTime()
    {
        yield return new WaitForSeconds(confusionDuration);
        pv.setMovementMultiplier(1f);
        Debug.Log("movemult: " + pv.getMovementMultiplier() + "; movespeed: " + pv.getMoveSpeed());
    }
}
