using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveAttack : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////

    [SerializeField] CircleCollider2D shockwave;
    [SerializeField] float shockwaveRadius;
    [SerializeField] float shockwaveSpeed;
    [SerializeField] float shockwaveStrength;
    [SerializeField] float shockwaveDamage;
    [SerializeField] bool poweredUp = false;
    private IceEffect iceEffect;

    private HashSet<PlayerMovement> playerMovements = new HashSet<PlayerMovement>();

    Vector3 scaleChange = new Vector3(0, 0, 0);

    void Start()
    {
        scaleChange.x = shockwaveSpeed;
        scaleChange.y = shockwaveSpeed;
        StartCoroutine(ExecuteShockwave());
    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || other.gameObject == transform.parent) return;

        other.GetComponent<PlayerVals>().IncrementHealth(-12);
        var target = other.GetComponent<PlayerMovement>();
        if (playerMovements.Contains(target)) return; // Prevent duplicate triggers

        Vector2 direction = (other.transform.position - transform.position).normalized;
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * shockwaveStrength * 150;
        //rb.AddForce(direction * shockwaveStrength * 1000);

        // Add buffed effects if applicable
        if (iceEffect != null)
        {
            int freezeFactor = Random.Range(0, 9);
            if (freezeFactor >= 5)iceEffect.ApplyFreeze(other.gameObject, 5f);
        }

        playerMovements.Add(target);
        target.SetCanMove(false);
        target.SetVelocityOverride(true);
        target.SetHitByShockwave(true);
    }

    private IEnumerator ExitStunAndDestroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f); // Stun time after shockwave
        foreach (var player in playerMovements)
        {
            // Only stop the stun if the player isn't frozen
            if (!player.gameObject.GetComponent<PlayerVals>().getFrozen()) player.SetCanMove(true);
        }
        playerMovements.Clear();
        Destroy(gameObject);
    }

    private IEnumerator ExecuteShockwave()
    {
        while (transform.localScale.x < shockwaveRadius)
        {
            yield return new WaitForSeconds(1 / shockwaveSpeed * 0.001f);
            if (shockwave.radius < 0.5f)
                GetComponent<CircleCollider2D>().radius += 0.01f * shockwaveSpeed;
            transform.localScale += 0.02f * scaleChange * shockwaveSpeed;
        }

        yield return ExitStunAndDestroy();
    }

    public void ApplyIceEffect(IceItem iceItem)
    {
        iceEffect = iceItem.GetIceEffect();
        AddIceVisuals();
    }

    public void AddIceVisuals()
    {
        // ADD CODE HERE
    }
}
