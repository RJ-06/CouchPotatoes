using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    [SerializeField] ItemSO itemSO;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Instantiate(itemSO.itemPrefab, other.gameObject.transform);
            Debug.Log("Picked up item!");
            Destroy(gameObject);
        }
    }
}
