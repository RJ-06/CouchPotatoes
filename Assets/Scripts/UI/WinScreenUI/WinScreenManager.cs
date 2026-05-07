using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI winnerText;
    string playerHasWon = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHasWon = GameManager.winningPlayer;
        if (winnerText != null) 
        {
            winnerText.text = playerHasWon + "has won!";

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ChangeScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }
}
