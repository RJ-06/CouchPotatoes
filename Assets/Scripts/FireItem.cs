using UnityEngine;

public class FireItem : ItemAttributes
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    //[SerializeField] ItemManager itemManager;
    [SerializeField] FireEffect fireEffect;

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


