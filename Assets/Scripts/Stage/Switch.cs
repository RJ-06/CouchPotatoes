using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    [Tooltip("Switching between the on and off states respectively")]
    UnityEvent actionOne;
    UnityEvent actionTwo;
    [Tooltip("A list of tags for objects that can enable the switch (ie attacks)")]
    [SerializeField] string[] switchTags;
    private bool currState = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < switchTags.Length; i++) 
        {
            if (collision.CompareTag(switchTags[i])) 
            {
                if (currState) { actionOne.Invoke(); }
                else { actionTwo.Invoke(); }
                currState = !currState;
            }
        }
    }
}
