using System.Collections;
using UnityEngine;

public class ShockwaveAttack : MonoBehaviour
{
    [SerializeField] CircleCollider2D shockwave;
    [SerializeField] float shockwaveRadius = 2f;
    [SerializeField] float shockwaveSpeed = 1f;
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

    private IEnumerator ExecuteShockwave()
    {
        while(transform.localScale.x < shockwaveRadius)
            {
                yield return new WaitForSeconds(1 / shockwaveSpeed * 0.001f);
                if(shockwave.radius < 0.5f)
                    GetComponent<CircleCollider2D>().radius += 0.01f * shockwaveSpeed;
                transform.localScale += 0.02f * scaleChange * shockwaveSpeed;
            }
    }
}
