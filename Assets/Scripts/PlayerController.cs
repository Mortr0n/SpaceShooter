using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    public GameObject plasma;
    public float moveSpeed =  25;

   

    private float boundaryX = 20f;
    private float minZ = -20f;
    private float maxZ = 3;
    private Vector3 velocity;

    void Start()
    { 
        playerRB = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(inputX, 0, inputZ) * moveSpeed * Time.deltaTime;

        if (transform.position.x > boundaryX)
        {
            transform.position = new Vector3(boundaryX, 3, transform.position.z);
        }
        if (transform.position.x < -boundaryX)
        {
            transform.position = new Vector3(-boundaryX, 3, transform.position.z);
        }
                
        if (transform.position.z < minZ)
        {
            transform.position = new Vector3(transform.position.x, 3, minZ);
        }
        if (transform.position.z > maxZ)
        {
            transform.position = new Vector3(transform.position.x, 3, maxZ);
        }
        else
        {
            
            transform.Translate(movement);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) )
        {
            FirePlasmaBall();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        switch (tag)
        {
            case "Enemy":
                Debug.Log("Enemy Collision");
                break;
            
            case "Asteroid":
                Debug.Log("Asteroid Collision");
                break;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        switch (tag)
        {
            case "PowerUp":
                Debug.Log("PowerUp Collected");
                Destroy(other.gameObject);
                break;
            case "PowerBomb":
                Debug.Log("PowerBomb Hit!");
                Destroy(other.gameObject);
                break;
            case "Laser":
                Debug.Log("Laser Hit!");
                Destroy(other.gameObject); 
                break;
        }
    }

    private void FirePlasmaBall()
    {
        Vector3 playerRightGun = new Vector3(.7f, .4f, .8f);
        Instantiate(plasma, playerRB.transform.position + playerRightGun, transform.rotation);

        Vector3 playerLeftGun = new Vector3(-.7f, .4f, .8f);
        Instantiate(plasma, playerRB.transform.position + playerLeftGun, transform.rotation);

        //GameObject rock = Instantiate(spaceRocks[idx], GenerateRandomLoc(), spaceRocks[idx].transform.rotation);
        //rock.transform.position = GenerateRandomLoc();
    }

}
