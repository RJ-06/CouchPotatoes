using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject resumeButton;

    public GameObject optionsMenu;

    public GameObject background;

    public TextMeshProUGUI countdownText;

    private GameObject lastSelectedButton;

    private bool isCountingDown = false;

    private MenuState currentState = MenuState.Pause;

    public enum MenuState
    {
        Pause,
        Options
    }

    void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !GameIsPaused && !isCountingDown)
        {
            Pause();
        }
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

    void Pause()
    {
        background.SetActive(true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

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
