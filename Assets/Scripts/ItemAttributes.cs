using UnityEngine;

public class ItemAttributes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // check if something can be used with fire powerup using ItemManager
    public virtual bool AffectedByFire(){
        return false;
    }
}
