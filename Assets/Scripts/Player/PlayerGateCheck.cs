using UnityEngine;

public class PlayerGateCheck : MonoBehaviour
{
    [SerializeField] Collider2D playerCollider;
    bool inGate = false;

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
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (inGate && (collision.gameObject.CompareTag("Moving Platform Gate") || collision.gameObject.CompareTag("Land Gate")))
        {
            inGate = false;
            playerCollider.enabled = true;
            Debug.Log("Error possible in PlayerGateCheck");
        }
    }

    public bool GetInGate() => inGate;
    public void SetInGate(bool state) => inGate = state;
}
