using UnityEngine;

public class WeakAttack : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    [SerializeField] int weakDamage = 7;
    [SerializeField] float knockbackForceStrength;
    private float damageMult = 1f;
    private IceEffect iceEffect;
    private bool attacked = false, iceBuffExists = false;

    void Start()
    {
        if (transform.parent.gameObject.GetComponent<PlayerItems>().CheckIceItem())
        {
            damageMult *= 2f;
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            iceBuffExists = true;

            ItemAttributes iceAttributes = transform.parent.gameObject.GetComponent<ItemManager>().GetAttributes().Find(item => item.name == "IceItem(Clone)");
            IceItem iceItem = iceAttributes as IceItem;
            if (iceItem != null)
            {
                iceEffect = iceItem.GetIceEffect();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!attacked)
        {
            if (other.CompareTag("Player") && other.gameObject != transform.parent)
            {
                PlayerVals target = other.GetComponent<PlayerVals>();
                PlayerMovement movement = other.GetComponent<PlayerMovement>();
                target.IncrementHealth(-weakDamage * (int)damageMult);

                // Handle freeze if necessary
                if (iceBuffExists && iceEffect != null)
                {
                    int chance = Random.Range(1, 5);
                    if (chance == 1)
                    {
                        iceEffect.ApplyFreeze(other.gameObject, 5f);
                    }
                }

                Rigidbody2D targetrb = other.GetComponent<Rigidbody2D>();
                Vector3 knockbackDir = other.transform.position - transform.parent.transform.position;
                movement.SetVelocityOverride(true);
                movement.SetPushedVelocity(knockbackDir.normalized * knockbackForceStrength);
                targetrb.linearVelocity = knockbackDir.normalized * knockbackForceStrength;
                attacked = true;
                movement.ExecutePush(0.5f);
            }
        }
    }
}
