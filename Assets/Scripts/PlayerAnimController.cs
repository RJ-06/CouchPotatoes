using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimController : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////

    // Player components
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] PlayerMovement playerMovement;
    Vector2 dirFacing;
    public int currSprite = 0;

    //DIFFERENT POTATOES
    [Tooltip("0: down, 1: right, 2: left, 3: up")]
    [SerializeField] Sprite[] playerOne;
    [SerializeField] Sprite[] playerTwo;
    [SerializeField] Sprite[] playerThree;
    [SerializeField] Sprite[] playerFour;
    [SerializeField] Sprite[] playerFive;
    [SerializeField] Sprite[] playerSix;

    Sprite[][] sprites;

    [SerializeField] SpriteRenderer hatSprite;
    [SerializeField] Sprite[] hats;
    [SerializeField] float[] hatOffset;
    [SerializeField] Transform hatTransform;
    public int currHat = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hatSprite.transform.position = hatTransform.position;
        sprites = new Sprite[6][];

        sprites[0] = playerOne;
        sprites[1] = playerTwo;
        sprites[2] = playerThree;
        sprites[3] = playerFour;
        sprites[4] = playerFive;
        sprites[5] = playerSix;
    }

    // Update is called once per frame
    void Update()
    {

        dirFacing = playerMovement.lastMoveDir;
        faceDirection();
    }

    void faceDirection()
    {
        if (dirFacing == Vector2.zero)
        {
            playerSprite.sprite = sprites[currSprite][0];
        }
        else if (dirFacing.y < 0)
        {
            playerSprite.sprite = sprites[currSprite][0];
        }
        else if (dirFacing.x > 0)
        {
            playerSprite.sprite = sprites[currSprite][1];
        }
        else if (dirFacing.x < 0)
        {
            playerSprite.sprite = sprites[currSprite][2];
        }
        else if (dirFacing.y > 0)
        {
            playerSprite.sprite = sprites[currSprite][3];
        }
    }

    void OnChangeHat()
    {
        currHat++;
        if (currHat >= hats.Length)
        {
            currHat = 0;
        }
        hatSprite.sprite = hats[currHat];
        hatSprite.transform.position = new Vector2(hatTransform.position.x, hatTransform.position.y + hatOffset[currSprite]);
    }

    public int GetCurrSprite() => currSprite;
    public void ChangeCurrSprite(int num)
    {
        currSprite = num;
    }
}
