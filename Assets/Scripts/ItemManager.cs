using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    [SerializeField] List<ItemSO> items;
    [SerializeField] List<ItemAttributes> itemAttributes;

    public List<ItemSO> GetItems() => items;
    public List<ItemAttributes> GetAttributes() => itemAttributes;
}
