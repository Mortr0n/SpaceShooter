using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    public GameObject plasma;

    public float moveSpeed =  25;
    public float weaponPauseTime = .8f;
    public int plasmaFireTimes = 3;

   

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
            StartCoroutine(PlasmaFireWithPause(plasmaFireTimes));
            //FirePlasmaBall();
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
            case "EnemyLaser":
                Debug.Log("Enemy Laser Hit!");
                Destroy (other.gameObject);
                KillPlayer();
                break;
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Player Death");
        Destroy(gameObject);
    }

    private void FirePlasmaBall(int side)
    {

        if (side == 0 || side % 2 == 0)
        {
            
            Vector3 playerRightGun = new Vector3(.7f, .4f, .8f);
            Debug.Log("Right: " + playerRightGun );
            Instantiate(plasma, playerRB.transform.position + playerRightGun, transform.rotation);
        }
        else
        {
            
            Vector3 playerLeftGun = new Vector3(-.7f, .4f, .8f);
            Debug.Log("Left: " + playerLeftGun);
            Instantiate(plasma, playerRB.transform.position + playerLeftGun, transform.rotation); 
        }
    }

    IEnumerator PlasmaFireWithPause(int amt)
    { 
        for( int i = 0; i < amt; i++) {
            FirePlasmaBall(i);
            yield return new WaitForSeconds(weaponPauseTime);
        }
    }
    


}
