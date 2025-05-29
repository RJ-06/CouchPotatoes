using UnityEngine;
using UnityEngine.Tilemaps;

public class Gate : MonoBehaviour
{
    [SerializeField] TilemapCollider2D gatePreventWalkOff, gateFallable;

    // IMPORTANT: Since tilemap rigidbodies are typically static, they won't collide with each other by default. For gating functionality to
    // work, one of the tilemaps in the collision MUST have their rigidbody set to kinematic!!! For purposes of convention, add the kinematic
    // rigidbodies to gate tilemaps on moving platforms.

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision detected from a gate");
        if (collision.CompareTag("Platform Gate"))
        {
            //Debug.Log("Collision was between two gates");
            gatePreventWalkOff.enabled = false;
            gateFallable.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform Gate"))
        {
            //Debug.Log("Two gates exited contact");
            gatePreventWalkOff.enabled = true;
            gateFallable.enabled = true;
        }
    }
}
