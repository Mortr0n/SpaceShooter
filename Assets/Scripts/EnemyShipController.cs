using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    public GameObject laser;

    private float laserTimer = 2f;
    private float largeShipLaserZ = -1.5f;
    private float largeShipAltLaserX = -1.5f;
    public float damageAmount;

    void Start()
    {
        StartCoroutine(FireLaser());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FireLaser()
    {
        yield return new WaitForSeconds(laserTimer);

        //Debug.Log(gameObject.name + " name ");
        //Debug.Log(gameObject.name == "LargeEnemyShip(Clone)"); 

        if (gameObject.name == "LargeEnemyShip(Clone)")
        {
            //Debug.Log("Firing Laser Hopefully: " + gameObject.name);
            Vector3 shipOffset;
            int offsetVar = Random.Range(0, 2);
            Debug.Log(offsetVar + "OFFSET");
            if (offsetVar == 0)
            {

                shipOffset = new Vector3(-.2f, -0f, largeShipLaserZ);
            }
            else
            {
                shipOffset = new Vector3(largeShipAltLaserX, -0f, largeShipLaserZ);
            }
            

            Instantiate(laser, transform.position + shipOffset, transform.rotation * Quaternion.Euler(0, 90, 0));
        }
        else
        {
            Instantiate(laser, transform.position, transform.rotation * Quaternion.Euler(0, 90, 0));
        }
        StartCoroutine(FireLaser());
    }

    

}
