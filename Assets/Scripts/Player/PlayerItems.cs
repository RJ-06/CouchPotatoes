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
    public Vector2 shootDir;

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

    // Potato gun item
    [SerializeField] float potatoGunCooldown;
    [SerializeField] float potatoGunCooldownTimer;
    private bool potatoGunUsed = false;
    private Transform potatoGunItem;
    GameObject potatoGunEquipped = null;

    [SerializeField] GameObject PotatoProjectilePrefab;
    [SerializeField] GameObject PotatoGunPrefab;
    [SerializeField] GameObject ShellEjectPrefab;
    Animator animator;


    // Other
    bool canAttack = true;


    void Start()
    {
        pv = GetComponent<PlayerVals>();
        movement = GetComponent<PlayerMovement>();
        shockwaveCooldown = pv.getAttackCooldown();
    }

    private void Update()
    {
        
        if (potatoGunItem == null)
            potatoGunItem = transform.Find("PotatoGunItem(Clone)");

        if (potatoGunItem != null)
        {
            if (!potatoGunEquipped)
            {
                potatoGunEquipped = Instantiate(PotatoGunPrefab, this.transform.position + new Vector3(0f, -0.2f, 10), Quaternion.identity, this.transform);
                animator = potatoGunEquipped.GetComponent<Animator>();
            }
        }
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

        if (potatoGunUsed && Mathf.Abs(potatoGunCooldownTimer) >= 0.0001)
        {
            potatoGunCooldownTimer -= Time.fixedDeltaTime;
        }
        else if (potatoGunUsed)
        {
            potatoGunUsed = false;
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
            else if (potatoGunItem != null)
            {
                if (!potatoGunUsed)
                {
                    potatoGunUsed = true;
                    potatoGunCooldownTimer = potatoGunCooldown;
                    PotatoGunAttack();
                    animator.SetTrigger("Shot");


                    GameObject shellEject = Instantiate(ShellEjectPrefab, potatoGunEquipped.transform.position, Quaternion.identity);

                    float shellEjectAngle = potatoGunEquipped.transform.eulerAngles.y == 0 ? 125.0f : 55.0f;
                    float gunAngle = potatoGunEquipped.transform.eulerAngles.z;
                    Vector2 shellEjectDir = new Vector2(Mathf.Cos((gunAngle + shellEjectAngle) * Mathf.Deg2Rad), Mathf.Sin((gunAngle + shellEjectAngle) * Mathf.Deg2Rad));
                    shellEject.GetComponent<Rigidbody2D>().AddForce(shellEjectDir * 25.0f);
                    shellEject.GetComponent<Rigidbody2D>().AddTorque(Random.Range(500.0f, 2000.0f));
                    Destroy(shellEject, 1.0f);
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

    public void OnAim(InputValue val) // Aim the potato with the right joystick on controller
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

    IEnumerator SlownessTime()
    {
        yield return new WaitForSeconds(slownessDuration);
        pv.setMovementMultiplier(1f);
    }

    //////////////////////////////////
    ///////// Getters/Setters ////////
    //////////////////////////////////

    public ItemManager GetItemManager() => itemManager;

    public void SetIceItem(IceItem item) => iceItem = item;

    public bool GetCanAttack() => canAttack;
    public void SetCanAttack(bool state) => canAttack = state;

    /////////////////////////////////
    /////// POTATO GUN ATTACK ///////
    /////////////////////////////////

    private void PotatoGunAttack()
    {
        Vector3 initPos;

        if (shootDir != Vector2.zero)
        {
            Vector2 normalizedShootDir = shootDir.normalized;
            initPos = new Vector3(
                transform.position.x + normalizedShootDir.x,
                transform.position.y + normalizedShootDir.y,
                transform.position.z
                );
        }
        else
        {
            Vector2 playerLastMoveDir = this.GetComponent<PlayerMovement>().lastMoveDir;
            initPos = transform.position + new Vector3(playerLastMoveDir.x, playerLastMoveDir.y, 0);
        }

        Instantiate(PotatoProjectilePrefab, initPos, Quaternion.Euler(new Vector3(0, 0, 0)), this.transform);

    }
}


