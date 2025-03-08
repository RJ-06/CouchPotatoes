using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    
    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
        
    }

    public void OpenOptions(GameObject defaultButton) {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(defaultButton);
    }

    public void CloseOptions(GameObject defaultButton) {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(defaultButton);
    }


    public void QuitApp() {
        Application.Quit();
        Debug.Log("Application has quited");
    }
}
