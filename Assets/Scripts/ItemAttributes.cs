using UnityEngine;

public class ItemAttributes : MonoBehaviour
{
    public ItemManager itemManager;

    public virtual void Awake()
    {
        Debug.Log("Base class start method run");
        itemManager = transform.parent.GetComponent<PlayerItems>().GetItemManager();
        Debug.Log(itemManager.gameObject);
        itemManager.GetAttributes().Add(this);
        Debug.Log(itemManager.GetAttributes());
    }

    // Check if something can be used with fire powerup using ItemManager
    public virtual bool AffectedByFire()
    {
        return false;
    }

    public virtual bool AffectedByIce()
    {
        return false;
    }

    public virtual void ApplyIceBuff() { return; }
}
