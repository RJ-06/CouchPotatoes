using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] List<ItemSO> items;

    public List<ItemSO> getItems(){
        return items;
    }
}
