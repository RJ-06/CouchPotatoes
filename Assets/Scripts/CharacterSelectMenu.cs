using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectMenu : MonoBehaviour
{
    [SerializeField] int playerCount = 0;
    [SerializeField] TextMeshProUGUI playerCountText;
    [SerializeField] int nextScene;
    public void OnAddPlayer() 
    {
        playerCount++;
    }

    public void OnRemovePlayer() 
    {
        playerCount--;
    }

    public void UpdatePlayerCountText() 
    {
        playerCountText.text = "Players: " + playerCount;
    }

    public void OnNextSceneClick() 
    {
        PlayerManager.playerCount = playerCount;   
        SceneManager.LoadScene(nextScene);
    }
    
}
