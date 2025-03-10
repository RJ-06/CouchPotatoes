using UnityEngine;

public class WeakAttack : MonoBehaviour
{
    [SerializeField] int weakDamage = 1;
    [SerializeField] float knockbackForceStrength = 0.5f;
    private bool attacked = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!attacked) {
            Debug.Log(other.CompareTag("Player"));
            Debug.Log(other.gameObject != transform.parent);
            if (other.CompareTag("Player") && other.gameObject != transform.parent)
            {
                PlayerVals target = other.GetComponent<PlayerVals>();
                target.IncrementHealth(-weakDamage);
                Rigidbody2D targetrb = other.GetComponent<Rigidbody2D>();
                Vector3 knockbackDir = other.transform.position - transform.position;
                targetrb.AddForce(knockbackDir.normalized * knockbackForceStrength);
                Destroy(gameObject);
            }
            attacked = true;
        }
    }
}
