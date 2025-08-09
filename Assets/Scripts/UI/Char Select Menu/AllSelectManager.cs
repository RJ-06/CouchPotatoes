using System.Collections.Generic;
using UnityEngine;

public class AllSelectManager : MonoBehaviour
{
    [SerializeField] GameObject selectCharacterPrefab;
    [SerializeField] Transform selectGroupTransform;

    private List<SelectCharacter> selectCharacters;
    private List<GameObject> playerCharacters;
    int charCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayerConnected()
    {
        charCount++;
        Instantiate(selectCharacterPrefab, selectGroupTransform);
        selectCharacters.Add(selectCharacterPrefab.GetComponent<SelectCharacter>());
    }


}
