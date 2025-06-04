using UnityEngine;

public class DamageableObj : MonoBehaviour
{
    [SerializeField] string[] tagsThatCanDamage;
    [SerializeField] MonoBehaviour[] componentsThatCanDamage;
    [SerializeField] int health = 1;

    [SerializeField] FXManager fx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider's tag is in the list of tags that can damage this object
        foreach (string tag in tagsThatCanDamage)
        {
            if (collision.CompareTag(tag))
            {
                TakeDamage();
                return; // Exit after taking damage to avoid multiple hits
            }
        }
        // Check if the collider's component is in the list of components that can damage this object
        foreach (MonoBehaviour component in componentsThatCanDamage)
        {
            if (collision.GetComponent(component.GetType()) != null)
            {
                TakeDamage();
                return; // Exit after taking damage to avoid multiple hits
            }
        }
    }

    void TakeDamage() 
    {
        health--;
        if (health <= 0) 
        {
            //PLAY PARTICLE EFFECTS HERE USING FXMANAGER
            Destroy(gameObject);
        }
    }


}
