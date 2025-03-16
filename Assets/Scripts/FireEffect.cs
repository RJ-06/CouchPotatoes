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

        while (elapsed < burnDuration){
            elapsed += Time.deltaTime;
            playerBurned.GetComponent<PlayerVals>().IncrementHealth(-1);
            Debug.Log(playerBurned.GetComponent<PlayerVals>().getHealth());
            yield return new WaitForSeconds(1f);
        }
    }
}

