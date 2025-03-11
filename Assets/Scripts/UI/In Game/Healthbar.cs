using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    public GameObject player;
    private Vector3 offset = new Vector3(0, 0.9f, 0);

    // NOTE TO JASON: I have commented out the problematic lines because they are causing compiler errors!
    // To access the getHealth function, you need to go through the player GameObject's PlayerVals component
    // i.e. for some player Player do:
    // Player.GetComponent<PlayerVals>()

    private void Start() {
        /*slider.maxValue = player.getHealth();
        slider.value = player.getHealth();*/

    }
    void LateUpdate () 
    {
        if (player == null) return;

        transform.position = player.transform.position + offset;

        //slider.value = player.getHealth();
    }

}
