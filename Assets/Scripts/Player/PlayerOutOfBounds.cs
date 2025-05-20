using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerOutOfBounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fallable Always") && !transform.parent.gameObject.GetComponent<PlayerMovement>().GetFallInProgress())
        {
            transform.parent.gameObject.GetComponent<PlayerMovement>().SetFallInProgress(true);
            StartCoroutine(transform.parent.gameObject.GetComponent<PlayerMovement>().Fall());
        }
    }
}
