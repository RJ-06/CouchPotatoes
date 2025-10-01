using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    private int playerIndex; // Index of the selected character
    [SerializeField] TMPro.TextMeshProUGUI characterNumberText;

    [SerializeField] Image selectedCharSprite;
    [SerializeField] Image selectedHatSprite;

    [SerializeField] Sprite[] characterSprites;
    private int selectedCharacterIndex = 0;
    [SerializeField] Sprite[] hatSprites;
    private int selectedHatIndex = 0;

    private void Start()
    {
        selectedCharSprite.sprite = characterSprites[selectedCharacterIndex];
        selectedHatSprite.sprite = hatSprites[selectedHatIndex];
    }


    public void OnCharacterJoin(int charNumber)
    {
        playerIndex = charNumber;
        UpdateCharacterText();

    }

    public void OnClickCharLeft()
    {
        selectedCharacterIndex--;
        if (selectedCharacterIndex < 0)
        {
            selectedCharacterIndex = characterSprites.Length - 1;
        }
        UpdateSelection();
    }

    public void OnClickCharRight()
    {
        selectedCharacterIndex++;
        if (selectedCharacterIndex >= characterSprites.Length)
        {
            selectedCharacterIndex = 0;
        }
        UpdateSelection();
    }

    public void OnClickHatLeft()
    {
        selectedHatIndex--;
        if (selectedHatIndex < 0)
        {
            selectedHatIndex = hatSprites.Length - 1;
        }
        UpdateSelection();
    }

    public void OnClickHatRight()
    {
        selectedHatIndex++;
        if (selectedHatIndex >= hatSprites.Length)
        {
            selectedHatIndex = 0;
        }
        UpdateSelection();
    }

    void UpdateCharacterText()
    {
        characterNumberText.text = "Player: " + (playerIndex + 1).ToString();
    }

    private void UpdateSelection() 
    {
        selectedCharSprite.sprite = characterSprites[selectedCharacterIndex];
        selectedHatSprite.sprite = hatSprites[selectedHatIndex];
    }
}
