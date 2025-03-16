using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    // UI components
    public GameObject mainMenu;
    public GameObject optionsMenu;
    private GameObject lastSelectedButton;

    // States
    private MenuState currentState = MenuState.Main;
    private enum MenuState
    {
        Main,
        Options
    }

    private void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);        
    }

    public void GoToScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name cannot be empty!");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void OpenOptions(GameObject defaultButton)
    {
        if (currentState != MenuState.Main) return;
        
        lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(defaultButton);

        currentState = MenuState.Options;
    }

    public void CloseOptions(GameObject defaultButton)
    {
        if (currentState != MenuState.Options) return;

        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(lastSelectedButton ?? defaultButton);

        currentState = MenuState.Main;
    }

    public void QuitApp()
    {
        Debug.Log("Application has quit");
        Application.Quit();
    }
}
