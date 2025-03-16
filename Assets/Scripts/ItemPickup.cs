using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameObject item = Instantiate(itemSO.itemPrefab, other.gameObject.transform);
            item.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Item Pickup");
            Destroy(gameObject);
        }
    }
}
