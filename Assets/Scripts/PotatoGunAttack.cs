using UnityEngine;
using UnityEngine.InputSystem;

public class PotatoGunAttack : MonoBehaviour
{

    private Vector2 shootDir;
    Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 playerLastMoveDir = this.GetComponentInParent<PlayerMovement>().lastMoveDir;

        if (shootDir != Vector2.zero)
        {
            Vector2 normalizedShootDir = shootDir.normalized;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(normalizedShootDir.y, normalizedShootDir.x));
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

    private void OnAim(InputValue val) // Aim the potato with the right joystick on controller
    {
        shootDir = val.Get<Vector2>();
    }
}
