using UnityEngine;
using UnityEngine.InputSystem;

public class Demo_SkinSwitch : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    [SerializeField] Sprite[] sprites;
    private int currSelected = 0;

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.sprite = sprites[currSelected];
    }

    void OnChangeSkin() 
    {
        changePlayerSkin();
    }

    void changePlayerSkin() 
    {
        currSelected++;
        if (currSelected >= sprites.Length)
        {
            currSelected = 0;
        }
        spriteRend.sprite = sprites[currSelected];
    }
}
