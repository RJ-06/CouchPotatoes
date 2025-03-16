using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectMenu : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    [SerializeField] TextMeshProUGUI playerCountText;
    private int playerCount = 0;
    private int nextScene;
    
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
