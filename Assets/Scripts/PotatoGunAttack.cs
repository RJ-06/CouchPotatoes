using UnityEngine;
using UnityEngine.InputSystem;

public class PotatoGunAttack : MonoBehaviour
{
    private Vector2 playerLastMoveDir;
    private Vector2 playerShootDir = Vector2.zero;

    Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        playerLastMoveDir = new Vector2(1, 0);
        playerShootDir = Vector2.zero;

        if (this.GetComponentInParent<PlayerMovement>() != null)
            playerLastMoveDir = this.GetComponentInParent<PlayerMovement>().lastMoveDir;

        if (this.GetComponentInParent<PlayerItems>() != null)
            playerShootDir = this.GetComponentInParent<PlayerItems>().shootDir;

        if (playerShootDir != Vector2.zero)
        {
            Vector2 normalizedShootDir = playerShootDir.normalized;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(normalizedShootDir.y, normalizedShootDir.x) * Mathf.Rad2Deg);
        }
        else
        {
            if (playerLastMoveDir.x == 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 90);
                if (playerLastMoveDir.y < 0)
                    transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
            else if (playerLastMoveDir.y == 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                //transform.eulerAngles = new Vector3(0, 0, 0);
                if (playerLastMoveDir.x < 0)
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                if (playerLastMoveDir.x > 0 && playerLastMoveDir.y > 0)
                    transform.eulerAngles = new Vector3(0, 0, 45);
                else if (playerLastMoveDir.x < 0 && playerLastMoveDir.y > 0)
                    transform.eulerAngles = new Vector3(0, 180, 45);
                else if (playerLastMoveDir.x < 0 && playerLastMoveDir.y < 0)
                    transform.eulerAngles = new Vector3(0, 180, -45);
                else if (playerLastMoveDir.x > 0 && playerLastMoveDir.y < 0)
                    transform.eulerAngles = new Vector3(0, 0, -45);
            }
        }


        
        //transform.eulerAngles.z = MathplayerLastMoveDir
        //print(playerLastMoveDir);
    }


}
