using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PotatoProjectileAttack : MonoBehaviour
{

    Rigidbody2D rb;
    float speed = 100.0f;
    [SerializeField] int knockbackForceStrength = 5;
    [SerializeField] int potatoProjectileDamage = 10;

    private Vector2 playerLastMoveDir;
    private Vector2 playerShootDir = Vector2.zero;

    AudioSource src;
    [SerializeField] AudioClip shotSound, shellSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        src = GetComponent<AudioSource>();
        src.PlayOneShot(shotSound);
        src.PlayOneShot(shellSound);

        Vector2 projectileDir;
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), this.transform.parent.gameObject.GetComponent<BoxCollider2D>(), true);

        if (this.GetComponentInParent<PlayerMovement>() != null)
            playerLastMoveDir = this.GetComponentInParent<PlayerMovement>().lastMoveDir;

        if (this.GetComponentInParent<PlayerItems>() != null)
            playerShootDir = this.GetComponentInParent<PlayerItems>().shootDir;

        if (playerShootDir != Vector2.zero)
            projectileDir = playerShootDir.normalized;
        else
            projectileDir = playerLastMoveDir.normalized;


        rb = GetComponent<Rigidbody2D>();
        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        rb.AddForce(projectileDir * speed);
        rb.angularVelocity += Random.Range(-1.0f, 1.0f) * speed * 10;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == this.transform.parent.gameObject)
        {
            return;
        }
            
        if (!collision.gameObject.CompareTag("Player") || collision.gameObject == transform.parent) return;

        Vector2 knockbackDir = collision.gameObject.transform.position - transform.position;

        collision.gameObject.GetComponent<PlayerVals>().IncrementHealth(-potatoProjectileDamage);

        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDir * knockbackForceStrength);

        Destroy(gameObject);
    }

}
