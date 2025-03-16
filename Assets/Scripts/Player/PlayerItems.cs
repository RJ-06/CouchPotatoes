using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems: MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    // Player components
    private PlayerVals pv;
    private Transform shockwaveItem;
    [SerializeField] GameObject ShockwavePrefab;
    [SerializeField] GameObject weakAttackPrefab;

    // Weak attack
    [SerializeField] float weakCooldown;
    [SerializeField] float weakCooldownTimer;
    private bool weakUsed = false;
    // [SerializeField] float weakRange = 2f;
    // [SerializeField] int weakDamage = 1;
    // [SerializeField] float knockbackForceStrength = 0.5f;
    // [SerializeField] float arcAngle = 33f;
    // [SerializeField] int numRays = 5;
    // private LineRenderer lineRenderer;

    // Shockwave attack
    [SerializeField] float shockwaveCooldown;
    [SerializeField] float shockwaveCooldownTimer;
    private bool shockwaveUsed = false;

    void Start()
    {
        pv = GetComponent<PlayerVals>();
        shockwaveCooldown = pv.getAttackCooldown();

        // lineRenderer = gameObject.AddComponent<LineRenderer>();
        // lineRenderer.material = new Material(Shader.Find("Sprites/Default")); 
        // lineRenderer.startWidth = 0.05f;  
        // lineRenderer.endWidth = 0.05f;
        // lineRenderer.positionCount = 2;  
        // lineRenderer.startColor = Color.red;  
        // lineRenderer.endColor = Color.red;
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
        // Debug.Log("Weak Attack");
        Vector3 attackPos = new Vector3(
            transform.position.x + transform.forward.x, 
            transform.position.y + 1f, 
            transform.position.z + transform.forward.z
        );        
        // Debug.Log(attackPos);
        // Debug.Log(transform.position);
        GameObject weakAttack = Instantiate(weakAttackPrefab, attackPos, Quaternion.identity, transform.gameObject.transform);
        Destroy(weakAttack, 1f);
        // Vector3 dirAtk = transform.forward;
        // // get the angle between each ray
        // float arcStep = arcAngle / (numRays - 1);
        // for(int i = 0; i < numRays; i++)
        // {
        //     // direction of each ray within the overall arc
        //     float currAngle = (-arcAngle / 2) + (i * arcStep);
        //     Vector3 dir = Quaternion.Euler(0, currAngle, 0) * dirAtk;
        //     Vector3 endOfRay = transform.position + dir * weakRange;
        //     lineRenderer.SetPosition(0, transform.position);
        //     lineRenderer.SetPosition(1, endOfRay);
        //     RaycastHit hit;
        //     if(Physics.Raycast(transform.position, dir, out hit, weakRange))
        //     {
        //         hit.transform.GetComponent<PlayerVals>().IncrementHealth(-weakDamage);
        //         hit.transform.GetComponent<Rigidbody>().AddForce(dir * knockbackForceStrength, ForceMode.Impulse);
        //     }

        // }
    }
}
