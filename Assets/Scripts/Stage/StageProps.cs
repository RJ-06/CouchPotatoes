using UnityEngine;
public class StageProps : MonoBehaviour
{
    [SerializeField] Transform[] propPositions;
    [SerializeField] GameObject[] props;

    [SerializeField] float spawnFreq;
    [SerializeField] float spawnVariability;
    [SerializeField] int limObjects;
    int count;
    float spawnTimer;
    float timeToSpawn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(GameObject g in props)
        {
            if (g)
            {
                count++;
            }
        }

        if (count < limObjects)
        {
            timeToSpawn += Time.deltaTime;
            if (spawnTimer >= timeToSpawn)
            {
                timeToSpawn = spawnFreq + Random.Range(-spawnVariability, spawnVariability);
                spawnTimer = 0;
                int randPos;
                while (true) 
                {
                    randPos = Random.Range(0, propPositions.Length);
                    if (propPositions[randPos] == null) 
                    {
                        break;
                    }
                }
                int randProp = Random.Range(0, props.Length);
                Instantiate(props[randProp], propPositions[randPos].position, Quaternion.identity);
            }
        }


    }
}
