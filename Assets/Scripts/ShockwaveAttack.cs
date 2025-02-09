using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveAttack : MonoBehaviour
{
    [SerializeField] CircleCollider2D shockwave;
    [SerializeField] float shockwaveRadius = 2f;
    [SerializeField] float shockwaveSpeed = 1f;
    [SerializeField] float shockwaveStrength = 1f;

    private HashSet<PlayerMovement> playerMovements = new HashSet<PlayerMovement>();

    Vector3 scaleChange = new Vector3(0, 0, 0);
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scaleChange.x = shockwaveSpeed;
        scaleChange.y = shockwaveSpeed;
        StartCoroutine(ExecuteShockwave());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject != transform.parent)
        {
            float xDist = other.gameObject.transform.position.x - transform.parent.gameObject.transform.position.x;
            float yDist = other.gameObject.transform.position.y - transform.parent.gameObject.transform.position.y;
            float angle;

            if ((xDist < 0 && yDist < 0) || xDist < 0)
            {
                angle = Mathf.Atan(yDist / xDist) + Mathf.PI;
            }
            else if (yDist < 0)
            {
                angle = Mathf.Atan(yDist / xDist) + Mathf.PI * 2;
            }
            else
            {
                angle = Mathf.Atan(yDist / xDist);
            }

            Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(force * shockwaveStrength * 150);

            var target = other.GetComponent<PlayerMovement>();
            playerMovements.Add(target);
            target.SetCanMove(false);
        }
    }

    private IEnumerator ExitStunAndDestroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        yield return null;
        foreach (var player in playerMovements)
        {
            player.SetCanMove(true);
        }
        playerMovements.Clear();
        Destroy(gameObject);
    }

    private IEnumerator ExecuteShockwave()
    {
        while(transform.localScale.x < shockwaveRadius)
            {
                yield return new WaitForSeconds(1 / shockwaveSpeed * 0.001f);
                if(shockwave.radius < 0.5f)
                    GetComponent<CircleCollider2D>().radius += 0.01f * shockwaveSpeed;
                transform.localScale += 0.02f * scaleChange * shockwaveSpeed;
            }

        yield return ExitStunAndDestroy();
    }
}
