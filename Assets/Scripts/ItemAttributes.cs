using UnityEngine;

public class ItemAttributes : MonoBehaviour
{
    // Check if something can be used with fire powerup using ItemManager
    public virtual bool AffectedByFire(){
        return false;
    }
}
