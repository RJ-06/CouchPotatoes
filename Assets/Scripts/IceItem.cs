using UnityEngine;

public class IceItem : ItemAttributes
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////

    [SerializeField] IceEffect iceEffect;

    void Start()
    {
        transform.parent.gameObject.GetComponent<PlayerItems>().SetIceItem(this);
        iceEffect = gameObject.AddComponent<IceEffect>();
        ApplyPowerUp();
    }

    void Update()
    {

    }

    public void ApplyPowerUp()
    {
        Debug.Log("Trying to apply power-ups to object: " + itemManager.gameObject);
        foreach (ItemAttributes item in itemManager.GetAttributes())
        {
            if (item.AffectedByIce())
            {
                Debug.Log($"Applying ice effect to {item.name}");
                item.ApplyIceBuff();
            }
        }

    }

    public IceEffect GetIceEffect() => iceEffect;
}


