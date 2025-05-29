using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////

    // Player components
    private PlayerVals pv;
    private PlayerMovement movement;
    private Transform shockwaveItem;
    [SerializeField] ItemManager itemManager;
    [SerializeField] GameObject ShockwavePrefab;
    [SerializeField] GameObject weakAttackPrefab;
    [SerializeField] GameObject ConfusionItem;
    [SerializeField] IceItem iceItem;
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

    // Frenzy item
    [SerializeField] float frenzyBoost;
    [SerializeField] float frenzyCooldownDecrease;

    // Slowness item
    [SerializeField] float slownessDuration;

    // Clone item
    [SerializeField] float cloneDuration;
    [SerializeField] GameObject playerClonePrefab;

    //Giant item
    [SerializeField] float giantDuration;
    [SerializeField] float giantSizeBoost;
    [SerializeField] float giantHealthBoost;
    [SerializeField] float giantDamageBoost;
    [SerializeField] float giantSlowdown;

    // Other
    bool canAttack = true;


    void Start()
    {
        pv = GetComponent<PlayerVals>();
        movement = GetComponent<PlayerMovement>();
        shockwaveCooldown = pv.getAttackCooldown();
    }

    void FixedUpdate()
    {
        // Handle cooldowns and attack executions
        if (shockwaveUsed && Mathf.Abs(shockwaveCooldownTimer) >= 0.0001)
        {
            shockwaveCooldownTimer -= Time.fixedDeltaTime;
        }
        else if (shockwaveUsed)
        {
            shockwaveUsed = false;
        }

        if (weakUsed && Mathf.Abs(weakCooldownTimer) >= 0.00001)
        {
            weakCooldownTimer -= Time.fixedDeltaTime;
        }
        else if (weakUsed)
        {
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
        Transform frenzyItem = transform.Find("FrenzyItem(Clone)");
        if (frenzyItem != null)
        {
            pv.setMovementMultiplier(pv.getMovementMultiplier() * frenzyBoost);
            pv.setAttackCooldown(pv.getAttackCooldown() * frenzyCooldownDecrease);
            pv.setDashCooldown(pv.getDashCooldown() * frenzyCooldownDecrease);
            Destroy(frenzyItem.gameObject);
        }
        Transform slownessItem = transform.Find("SlownessItem(Clone)");
        if (slownessItem != null)
        {
            transform.gameObject.GetComponent<Rigidbody2D>().linearVelocity *= 0.5f;
            pv.setMovementMultiplier(0.5f);
            Destroy(slownessItem.gameObject);
            StartCoroutine("SlownessTime");
        }

        Transform giantItem = transform.Find("GiantItem(Clone)");
        if (giantItem != null)
        {
            transform.gameObject.transform.localScale *= giantSizeBoost;
            pv.setMovementMultiplier(pv.getMovementMultiplier() * giantSlowdown);
            pv.setAttackPoints((int)(pv.getAttackPoints() * giantDamageBoost));
            pv.setHealth((int)(pv.getHealth() + (int)(pv.getMaxHealth() * giantHealthBoost)));
            Destroy(giantItem.gameObject);
        }

        Transform cloneItem = transform.Find("CloneItem(Clone)");
        if (cloneItem != null)
        {
            // Get the item's position
            //Vector3 itemPosition = cloneItem.position;

            // Move player to left side of item
            //transform.position = itemPosition;

            GameObject clone = new GameObject();
            StartCoroutine(MakeClones(clone));

            // Set the initial moving direction of the clone as opposite to the player
            CloneBehavior cloneBehavior = clone.GetComponent<CloneBehavior>();
            if (cloneBehavior != null)
            {
                cloneBehavior.SetInitialDirection(-movement.lastMoveDir.normalized);
                cloneBehavior.SetMoveSpeed(pv.getMoveSpeed());
            }



            Destroy(cloneItem.gameObject);
            StartCoroutine(CloneLifespan(clone));
        }
    }


    ////////////////////////////
    ////////// INPUTS //////////
    ////////////////////////////

    private void OnAttack()
    {
        if (canAttack)
        {
            shockwaveItem = transform.Find("ShockwaveItem(Clone)");

            if (shockwaveItem != null && !shockwaveUsed) // Shockwave attack used
            {
                shockwaveUsed = true;
                shockwaveCooldownTimer = shockwaveCooldown;
                GameObject newShockwave = Instantiate(ShockwavePrefab, transform.position, Quaternion.identity, this.transform);

                // Apply necessary buffs to the new shockwave
                if (shockwaveItem.GetComponent<ShockwaveItem>().getIceBuff())
                {
                    newShockwave.GetComponent<ShockwaveAttack>().ApplyIceEffect(iceItem);
                }
            }
            else if (shockwaveItem == null && !weakUsed) // No other attack available, so do weak attack
            {
                weakCooldownTimer = weakCooldown;
                WeakAttack();
            }
        }
    }


    /////////////////////////////////
    ////////// WEAK ATTACK //////////
    /////////////////////////////////

    private void WeakAttack()
    {
        if (shootDir != Vector2.zero)
        {
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
        else
        {
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
        shootDir = val.Get<Vector2>() * pv.getSpeedSensitivityMultiplier();
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

    IEnumerator SlownessTime()
    {
        yield return new WaitForSeconds(slownessDuration);
        pv.setMovementMultiplier(1f);
    }

    IEnumerator GiantTime() 
    {
        yield return new WaitForSeconds(giantDuration);
        transform.parent.gameObject.transform.localScale /= giantSizeBoost;
        pv.setMovementMultiplier(pv.getMovementMultiplier() / giantSlowdown);
        pv.setAttackPoints((int)(pv.getAttackPoints() / giantDamageBoost));
        //pv.setHealth((int)(pv.getHealth() + (int)(pv.getMaxHealth() * giantHealthBoost)));
    }

    IEnumerator MakeClones(GameObject clone)
    {
        transform.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        movement.SetCanMove(false);
        transform.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        Vector2[] positions = new Vector2[]
        {
            new Vector2(-1, 1),  // Top left
            new Vector2(1, 1),   // Top right
            new Vector2(-1, -1), // Bottom left 
            new Vector2(1, -1),  // Bottom right
        };

        // Shuffle positions
        for (int i = positions.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector2 temp = positions[i];
            positions[i] = positions[j];
            positions[j] = temp;
        }

        // Apply force to the player first 
        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        float forceMagnitude = 18f;
        playerRb.AddForce(positions[0] * forceMagnitude, ForceMode2D.Impulse);

        for (int i = 1; i < 4; i++)
        {
            // Spawn clone on right side of item
            GameObject newClone = Instantiate(playerClonePrefab, transform.position, transform.rotation);

            // Copy the sprite renderer of the player to its clone
            SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
            SpriteRenderer cloneSprite = newClone.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                cloneSprite.sprite = playerSprite.sprite;
                cloneSprite.material = playerSprite.material;
                cloneSprite.color = playerSprite.color;
                cloneSprite.flipX = playerSprite.flipX;
                cloneSprite.flipY = playerSprite.flipY;
            }

            // Set up clone behavior
            CloneBehavior cloneBehavior = newClone.GetComponent<CloneBehavior>();
            if (cloneBehavior != null)
            {
                cloneBehavior.SetInitialDirection(positions[i].normalized);
                cloneBehavior.SetMoveSpeed(pv.getMoveSpeed());
            }

            // Apply force to the clone 
            Rigidbody2D cloneRb = newClone.GetComponent<Rigidbody2D>();
            if (cloneRb != null)
            {
                cloneRb.AddForce(positions[i] * forceMagnitude, ForceMode2D.Impulse);
            }
        }

        movement.SetCanMove(true);
        transform.gameObject.GetComponent<BoxCollider2D>().enabled = true;

        yield return null;
    }

    IEnumerator CloneLifespan(GameObject clone)
    {
        yield return new WaitForSeconds(cloneDuration);
        if (clone != null)
            Destroy(clone);
    }

    //////////////////////////////////
    ///////// Getters/Setters ////////
    //////////////////////////////////

    public ItemManager GetItemManager() => itemManager;

    public void SetIceItem(IceItem item) => iceItem = item;

    public bool GetCanAttack() => canAttack;
    public void SetCanAttack(bool state) => canAttack = state;
}

