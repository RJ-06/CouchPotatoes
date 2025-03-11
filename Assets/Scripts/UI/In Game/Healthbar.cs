using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    public GameObject player;
    private Vector3 offset = new Vector3(0, 0.9f, 0);



    private void Start() {
        slider.maxValue = player.getHealth();
        slider.value = player.getHealth();

    }
    void LateUpdate () 
    {
        if (player == null) return;

        transform.position = player.transform.position + offset;

        slider.value = player.getHealth();
    }

}
