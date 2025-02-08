using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems: MonoBehaviour
{
    
    private Transform shockwaveItem;
    [SerializeField] float shockwaveCooldown = 1f;
    [SerializeField] float shockwaveCooldownTimer;
    private bool shockwaveUsed = false;
    [SerializeField] GameObject ShockwavePrefab;

    private PlayerInput playerInput;
    private PlayerVals pv;
    void Start()
    {
        pv = GetComponent<PlayerVals>();
        playerInput = GetComponent<PlayerInput>();
        shockwaveCooldown = pv.getAttackCooldown();
    }

    void FixedUpdate()
    {
        if (shockwaveUsed && Mathf.Abs(shockwaveCooldownTimer) >= 0.0001) {
            shockwaveCooldownTimer -= Time.fixedDeltaTime;
        } else if (shockwaveUsed) {
            shockwaveUsed = false;
        }
    }

    private void OnAttack()
    {
        shockwaveItem = transform.Find("ShockwaveItem(Clone)");

        if(shockwaveItem != null && !shockwaveUsed)
        {
            shockwaveUsed = true;
            shockwaveCooldownTimer = shockwaveCooldown;
            Debug.Log(shockwaveCooldownTimer + ", " + shockwaveCooldown);
            Instantiate(ShockwavePrefab, transform.position, Quaternion.identity, this.transform);
            Debug.Log("Attacked");
        }
    }
}
