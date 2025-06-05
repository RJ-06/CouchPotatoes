using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DEBUGRELOAD : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnForceReload() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
