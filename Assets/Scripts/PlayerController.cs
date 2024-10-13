using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // gameobjects
    private Rigidbody playerRB;
    public GameObject plasma;

    // player vars
    private float maxHealth = 100f;
    private float playerHealth;
    

    // movement vars
    public float moveSpeed =  25;
    private float boundaryX = 20f;
    private float minZ = -20f;
    private float maxZ = 3;
    private Vector3 velocity;

    // plasma weapon vars
    public float weaponPauseTime = .6f;
    public int plasmaFireTimes = 3;
    private float plasmaBurstPauseTime = .5f;
    public bool canFire = true;

    void Start()
    { 
        playerRB = GetComponent<Rigidbody>();
        playerHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(inputX, 0, inputZ) * moveSpeed * Time.deltaTime;
        
        // contain left to right movement in boundaries of map
        if (transform.position.x > boundaryX)
        {
            transform.position = new Vector3(boundaryX, 3, transform.position.z);
        }
        if (transform.position.x < -boundaryX)
        {
            transform.position = new Vector3(-boundaryX, 3, transform.position.z);
        }
            
        // contain forward and backwards movement
        if (transform.position.z < minZ)
        {
            transform.position = new Vector3(transform.position.x, 3, minZ);
        }
        if (transform.position.z > maxZ)
        {
            transform.position = new Vector3(transform.position.x, 3, maxZ);
        }
        else // looks like we can move!
        {
            transform.Translate(movement);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && canFire )
        {
            canFire = false; // for pause in between shots so they can't just spam fire button
            StartCoroutine(PlasmaFireWithPause(plasmaFireTimes));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        MoveDown dmgComponent = collision.gameObject.GetComponent<MoveDown>();

        switch (tag)
        {
            case "Enemy":
                Debug.Log("Enemy Collision");
                DamagePlayer(dmgComponent.GetDamage());
                break;
            
            case "Asteroid":
                Debug.Log("Asteroid Collision");
                DamagePlayer(dmgComponent.GetDamage());
                break;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        EnemyProjectiles enemyProjectile = other.gameObject.GetComponent<EnemyProjectiles>();
        RepairController repairController = other.gameObject.GetComponent<RepairController>();
        switch (tag)
        {
            case "PowerUp":
                Debug.Log("PowerUp Collected");
                if (repairController != null)
                {
                    maxHealth += repairController.MaxHealthIncAmount();
                    HealPlayer(repairController.MaxHealthIncAmount());
                }
                //Destroy(other.gameObject);
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
                if (enemyProjectile != null)
                {
                    float damageAmount = enemyProjectile.damageAmount;
                    DamagePlayer(damageAmount);
                } 
                else
                {
                    Debug.LogWarning("MoveDown component is missing or not set!");
                }
                //DamagePlayer(dmgComponent.GetDamage());
                Destroy(other.gameObject);
                break;
            case "Repair":
                Debug.Log("Repairing!");

                if (repairController != null)
                {
                    HealPlayer(repairController.healAmount);
                }
                else
                {
                    Debug.LogError("Repair controller not set or missing!");
                }
                break;
        }
    }

    private void HealPlayer(float amount)
    {
        if (playerHealth < maxHealth)
        {
            
            playerHealth += amount;
            if (playerHealth > maxHealth)
            {
                playerHealth = maxHealth;
            }
            Debug.Log("Healing for " + amount + " Health is at " + playerHealth + " and max is " + maxHealth);
        }
    }

    public void DamagePlayer(float amount)
    {
        Debug.Log("Damaging player for " + amount + " amount of damage, now at " + (playerHealth - amount) + " amount.");
        if (playerHealth > amount)
        {
            playerHealth -= amount;
        } else
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Player Death");
        Destroy(gameObject);
    }

    private void FirePlasmaBall(int side)
    {
        // choose side to fire from based on in passed in
        if (side == 0 || side % 2 == 0)
        {
            Vector3 playerRightGun = new Vector3(.7f, .4f, .8f);
            Instantiate(plasma, playerRB.transform.position + playerRightGun, transform.rotation);
        }
        else
        {
            Vector3 playerLeftGun = new Vector3(-.7f, .4f, .8f);
            Instantiate(plasma, playerRB.transform.position + playerLeftGun, transform.rotation); 
        }
    }

    IEnumerator PlasmaFireWithPause(int amt)
    {
        // multi shot plasma fire with pause in between shots
        canFire = false;    
        for (int i = 0; i < amt; i++)
        {
            FirePlasmaBall(i);
            yield return new WaitForSeconds(weaponPauseTime);
        }
        yield return new WaitForSeconds(plasmaBurstPauseTime);
        canFire = true;
    }
    


}
