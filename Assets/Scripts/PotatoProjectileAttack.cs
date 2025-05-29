using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PotatoProjectileAttack : MonoBehaviour
{

    Rigidbody2D rb;
    float speed = 100.0f;
    [SerializeField] int knockbackForceStrength = 5;
    [SerializeField] int potatoProjectileDamage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector2 playerLastMoveDir = this.GetComponentInParent<PlayerMovement>().lastMoveDir;

        rb = GetComponent<Rigidbody2D>();
        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        rb.AddForce(playerLastMoveDir * speed);
        rb.angularVelocity += Random.Range(-1.0f, 1.0f) * speed * 10;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || collision.gameObject == transform.parent) return;

        Vector2 knockbackDir = collision.gameObject.transform.position - transform.position;

        collision.gameObject.GetComponent<PlayerVals>().IncrementHealth(-potatoProjectileDamage);

        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDir * knockbackForceStrength);

        Destroy(gameObject);
    }

}
