using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerController : MonoBehaviour
{
    public float xRange = 12f;
    public float zPos = 13f;
    public float yPos = 3f;
    private float rockDelay = 1f;
    
    
    public GameObject[] spaceRocks;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RockTimer(rockDelay));
    }

    // Update is called once per frame 
    void Update()
    {
        
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

    IEnumerator RockTimer(float delay)
    {
        yield return new WaitForSeconds(delay); 
        //SpawnRock();
        GameObject rock;
        int idx = Random.Range(0, spaceRocks.Length);
        if (spaceRocks[idx] != null)
        {
            rock = Instantiate(spaceRocks[idx], GenerateRandomLoc(), spaceRocks[idx].transform.rotation);
            yield return new WaitForEndOfFrame();
            StartCoroutine(RockTimer(rockDelay));
            rock.transform.position = GenerateRandomLoc();
        }
    }
}
