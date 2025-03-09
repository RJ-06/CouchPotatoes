using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] List<ItemSO> items;
    [SerializeField] List<ItemAttributes> itemAttributes;

    public List<ItemSO> GetItems(){
        return items;
    }
    public List<ItemAttributes> GetAttributes(){
        return itemAttributes;
    }
}
