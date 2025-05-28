using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject resumeButton;
    public GameObject optionsMenu; // Reference to the options menu
    public GameObject background;
    public TextMeshProUGUI countdownText;
    private GameObject lastSelectedButton;
    private bool isCountingDown = false;
    private MenuState currentState = MenuState.Pause;
    private GameManager gameManager; // Reference to GameManager

    public enum MenuState
    {
        Pause,
        Options
    }

    void Start()
    {
        pauseMenu.SetActive(false);
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        // Get reference to GameManager
        gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        background.SetActive(false);
        StartCoroutine(CountdownToResume());
    }

    IEnumerator CountdownToResume()
    {
        isCountingDown = true;
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }

        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        isCountingDown = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        background.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;

        // Play pause sound effect if GameManager is available
        if (gameManager != null)
        {
            // You'll need to assign the pause sound clip in the inspector
            gameManager.PlaySFX(gameManager.testSFX);
        }

        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void OpenOptions(GameObject defaultButton)
    {
        if (currentState != MenuState.Pause) return;

        lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(defaultButton);

        currentState = MenuState.Options;
    }

    public void CloseOptions(GameObject defaultButton)
    {
        if (currentState != MenuState.Options) return;

        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(lastSelectedButton ?? defaultButton);

        currentState = MenuState.Pause;
    }
}
