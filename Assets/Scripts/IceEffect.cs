using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class IceEffect : ItemAttributes
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    [SerializeField] float frostbiteDuration;
    [SerializeField] float freezeDuration;
    
    void Start()
    {
        Debug.Log("Ice is working");
    }

    void Update()
    {
        
    }

    public void ApplyFrostbite(GameObject playerIced, float dur) {
        frostbiteDuration = dur;
        StartCoroutine(Frostbite(playerIced));
    }

    public void ApplyFreeze(GameObject playerFrozen, float dur) {
        freezeDuration = dur;
        StartCoroutine(Freeze(playerFrozen));
    }
    
    // STILL A COPY FROM FIRE!! Change later
    private IEnumerator Frostbite(GameObject playerIced) {
        float elapsed = 0f;

        while (elapsed < frostbiteDuration){
            elapsed += Time.deltaTime;
            playerIced.GetComponent<PlayerVals>().IncrementHealth(-1);
            Debug.Log(playerIced.GetComponent<PlayerVals>().getHealth());
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator Freeze(GameObject playerFrozen) {
        float elapsed = 0f;
        // I'd imagine it sucks to get frozen solid. 1/3 health loss *maniacal laugh*
        playerFrozen.GetComponent<PlayerVals>().IncrementHealth((int)(playerFrozen.GetComponent<PlayerVals>().getMaxHealth() / 3f));

        while (elapsed < freezeDuration) {
            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(1f);
        }
    }
}

