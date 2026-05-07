using System.Collections;
using UnityEngine;

public class FireEffect : ItemAttributes
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    [SerializeField] float burnDuration;
    [SerializeField] float burnDamagePerSecond;
    
    void Start()
    {
        Debug.Log("Fire is working.");
    }

    void Update()
    {
        
    }
    public void ApplyFireBurn(GameObject playerBurned, float dur, float dps){
        burnDuration = dur;
        burnDamagePerSecond = dps;

        StartCoroutine(BurnCoroutine(playerBurned));
    }    private IEnumerator BurnCoroutine(GameObject playerBurned){
        float elapsed = 0f;
        while (elapsed < burnDuration)
        {
            // Apply damage once per second based on burnDamagePerSecond
            playerBurned.GetComponent<PlayerVals>().IncrementHealth(-Mathf.RoundToInt(burnDamagePerSecond));
            Debug.Log(playerBurned.GetComponent<PlayerVals>().getHealth());
            elapsed += 1f;
            yield return new WaitForSeconds(1f);
        }
    }
}

