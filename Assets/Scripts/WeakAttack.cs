using UnityEngine;

public class WeakAttack : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    [SerializeField] int weakDamage = 7;
    [SerializeField] float knockbackForceStrength = 0.5f;
    private float damageMult = 1f;
    private bool attacked = false;

    void Start()
    {
        if (transform.parent.gameObject.GetComponent<PlayerItems>().CheckIceItem())
        {
            damageMult *= 2f;
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!attacked)
        {
            Debug.Log(other.CompareTag("Player"));
            Debug.Log(other.gameObject != transform.parent);
            if (other.CompareTag("Player") && other.gameObject != transform.parent)
            {
                PlayerVals target = other.GetComponent<PlayerVals>();
                target.IncrementHealth(-weakDamage * (int)damageMult);
                Rigidbody2D targetrb = other.GetComponent<Rigidbody2D>();
                Vector3 knockbackDir = other.transform.position - transform.position;
                targetrb.AddForce(knockbackDir.normalized * knockbackForceStrength);
                Destroy(gameObject);
            }
            attacked = true;
        }
    }
}
