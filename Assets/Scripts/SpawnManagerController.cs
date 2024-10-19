using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerController : MonoBehaviour
{
    public float xRange = 20;
    public float zPos = 35;
    public float yPos = 3f;
    private float rockDelay = 1f;
    private float powerUpDelay = 30f;
    private int spawnCounter = 0;
    private int difficultyUp = 30;
    
    
    public GameObject[] spaceRocks;
    public GameObject[] powerUps;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTimer(rockDelay));
        StartCoroutine(PowerUpTimer(1f));
    }

    private Vector3 GenerateRandomLoc()
    {
        float x = Random.Range(-xRange, xRange); 
        Vector3 randomPos = new Vector3(x, yPos, zPos);
        return randomPos;
    }

    void SpawnRock()
    {
        int idx = Random.Range(0, spaceRocks.Length);
        if (spaceRocks[idx] != null)
        {

            GameObject rock = Instantiate(spaceRocks[idx], GenerateRandomLoc(), spaceRocks[idx].transform.rotation);
            rock.transform.position = GenerateRandomLoc();
        }
    }

    IEnumerator SpawnTimer(float delay) 
    {
        yield return new WaitForSeconds(delay); 
        
        GameObject rock;
        int idx = Random.Range(0, spaceRocks.Length);
        if (spaceRocks[idx] != null)
        {
            spawnCounter++;
            rock = Instantiate(spaceRocks[idx], GenerateRandomLoc(), spaceRocks[idx].transform.rotation);
            yield return new WaitForEndOfFrame();


            if (spawnCounter >= difficultyUp)
            {
                
                spawnCounter = 0;
                if (rockDelay > .3f)
                {
                    difficultyUp += 30;
                    rockDelay -= .1f;
                }
                
            }

            StartCoroutine(SpawnTimer(rockDelay));
            rock.transform.position = GenerateRandomLoc();
        }
    }

    IEnumerator PowerUpTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject powerUp;
        //TODO: Instead of using multiples of an object to create percentage chances instead use random numbers and then grab an index based on a range that I can set ratio's for
        int idx = Random.Range(0, powerUps.Length);
        if (powerUps[idx] != null)
        {
            powerUp = Instantiate(powerUps[idx], GenerateRandomLoc(), powerUps[idx].transform.rotation);
            yield return new WaitForEndOfFrame();

            // Edit the powerUpDelay to make this faster or slower.  Could use an object to speed it up and then reduce it back or something
            StartCoroutine(PowerUpTimer(powerUpDelay));
            powerUp.transform.position = GenerateRandomLoc();
        }

    }
}
