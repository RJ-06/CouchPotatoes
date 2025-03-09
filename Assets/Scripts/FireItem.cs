using UnityEngine;

public class FireItem : ItemAttributes
{
    [SerializeField] ItemManager itemManager;
    [SerializeField] FireEffect fireEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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


