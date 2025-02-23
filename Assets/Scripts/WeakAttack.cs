using UnityEngine;

public class WeakAttack : MonoBehaviour
{
    [SerializeField] int weakDamage = 1;
    [SerializeField] float knockbackForceStrength = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject != transform.parent)
        {
            PlayerVals target = other.GetComponent<PlayerVals>();
            target.IncrementHealth(-weakDamage);
            Rigidbody targetrb = other.GetComponent<Rigidbody>();
            Vector3 knockbackDir = other.transform.position - transform.position;
            targetrb.AddForce(knockbackDir.normalized * knockbackForceStrength, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
