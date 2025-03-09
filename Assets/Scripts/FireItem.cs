using UnityEngine;

public class FireItem : ItemAttributes
{
    [SerializeField] private ItemManager itemManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ApplyPowerUp(){
        foreach (ItemSO item in itemManager.getItems())
        {
        }

    }
}


