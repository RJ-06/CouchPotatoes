using System.Collections;
using UnityEngine;

public class ShockwaveAttack : MonoBehaviour
{
    [SerializeField] CircleCollider2D shockwave;
    [SerializeField] float shockwaveRadius = 2f;
    [SerializeField] float shockwaveSpeed = 1f;
    [SerializeField] float shockwaveStrength = 1f;


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

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && other.gameObject != transform.parent)
        {
            float xDist = other.gameObject.transform.position.x - transform.parent.gameObject.transform.position.x;
            float yDist = other.gameObject.transform.position.y - transform.parent.gameObject.transform.position.y;
            float angle;

            if((xDist < 0 && yDist < 0) || xDist < 0)
            {
                angle = Mathf.Atan(yDist / xDist) + Mathf.PI;
            } 
            else if(yDist < 0)
            {
                angle = Mathf.Atan(yDist / xDist) + Mathf.PI * 2;
            } 
            else
            {
                angle = Mathf.Atan(yDist / xDist);
            }
            
            Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(force * shockwaveStrength * 150);
        }
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
        Destroy(gameObject);
    }
}
