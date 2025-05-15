using UnityEngine;

public class IceItem : ItemAttributes
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    [SerializeField] ItemManager itemManager;
    [SerializeField] IceEffect IceEffect;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ApplyPowerUp(){
        foreach (ItemAttributes item in itemManager.GetAttributes())
        {
            if (item.AffectedByFire())
            {
                Debug.Log($"Applying fire effect to {item.name}");
                // Apply fire effect to the relevant weapon
                // Change the 2d collision detection so that it also 
                // adds burning status to the affected player(s)

            }
        }

    }
}


