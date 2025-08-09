using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    private int playerIndex; // Index of the selected character
    [SerializeField] TMPro.TextMeshProUGUI characterNumberText;

    [SerializeField] Image selectedCharSprite;
    [SerializeField] Image selectedHatSprite;

    [SerializeField] Image[] characterSprites;
    private int selectedCharacterIndex = 0;
    [SerializeField] Image[] hatSprites;
    private int selectedHatIndex = 0;

    private void Start()
    {
        selectedCharSprite = characterSprites[selectedCharacterIndex];
        selectedHatSprite = hatSprites[selectedHatIndex];
    }


    public void OnCharacterJoin(int charNumber) 
    {
        playerIndex = charNumber;
        updateCharacterText();

    }

    public void OnClickCharLeft() 
    {
        selectedCharacterIndex--;
        if (selectedCharacterIndex < 0) 
        {
            selectedCharacterIndex = characterSprites.Length - 1; 
        }
    }

    public void OnClickCharRight()
    {
        selectedCharacterIndex++;
        if (selectedCharacterIndex >= characterSprites.Length)
        {
            selectedCharacterIndex = 0;
        }
    }

    public void OnClickHatLeft()
    {
        selectedHatIndex--;
        if (selectedHatIndex < 0)
        {
            selectedHatIndex = hatSprites.Length - 1;
        }
    }

    public void OnClickHatRight()
    {
        selectedHatIndex++;
        if (selectedHatIndex >= hatSprites.Length)
        {
            selectedHatIndex = 0;
        }
    }

    void updateCharacterText() 
    {
        characterNumberText.text = "Player: " + (playerIndex + 1).ToString();
    }
}
