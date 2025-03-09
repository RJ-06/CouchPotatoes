using System.Collections;
using UnityEngine;

public class FireEffect : ItemAttributes
{
    [SerializeField] float burnDuration;
    [SerializeField] float burnDamagePerSecond;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ApplyFireBurn(GameObject playerBurned, float dur, float dps){
        burnDuration = dur;
        burnDamagePerSecond = dps;

        StartCoroutine(BurnCoroutine(playerBurned));
    }
    void Start()
    {
        Debug.Log("Fire is working.");
    }
    private IEnumerator BurnCoroutine(GameObject playerBurned){
        float elapsed = 0f;

        while (elapsed < burnDuration){
            elapsed += 1f;
            playerBurned.GetComponent<PlayerVals>().IncrementHealth(-1);
            Debug.Log(playerBurned.GetComponent<PlayerVals>().getHealth());
            yield return new WaitForSeconds(1f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

