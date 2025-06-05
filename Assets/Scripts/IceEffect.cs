using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class IceEffect : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    [SerializeField] float frostbiteDuration;
    
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
        StartCoroutine(Freeze(playerFrozen, dur));
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

    private IEnumerator Freeze(GameObject playerFrozen, float freezeDuration)
    {
        Debug.Log("Freeze coroutine started");

        playerFrozen.GetComponent<PlayerVals>().setFrozen(true);
        playerFrozen.GetComponent<SpriteRenderer>().color = Color.blue;
        playerFrozen.GetComponent<PlayerMovement>().SetCanMove(false);
        // I'd imagine it sucks to get frozen solid. 1/3 health loss *maniacal laugh*
        Debug.Log("Health lost");
        playerFrozen.GetComponent<PlayerVals>().IncrementHealth(-13);

        Debug.Log("Waiting for " + freezeDuration + " seconds");
        yield return new WaitForSeconds(freezeDuration);

        playerFrozen.GetComponent<PlayerMovement>().SetCanMove(true);
        playerFrozen.GetComponent<SpriteRenderer>().color = Color.white;
        Debug.Log("Freeze coroutine finished");
    }
}

