using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    // gameobjects
    private Rigidbody playerRB;
    public GameObject plasma;

    // player Stat vars
    private float maxHealth = 100f;
    [SerializeField]
    private float playerHealth;
    private float armorVal = 1;

    // Player Level vars
    private float playerLevel = 0;
    private float playerExpToLevel = 100f;
    private float playerExp = 0;
    private float expModifier = 1f;
    private float expToLevelModifier = 1.1f;
    private Action[] levelUpActions;


    // movement vars
    public float moveSpeed =  8;
    private float boundaryX = 20f;
    private float minZ = -20f;
    private float maxZ = 3;
    private Vector3 velocity;

    // plasma weapon vars
    public float weaponPauseTime = .6f;
    public int plasmaFireTimes = 3;
    private float plasmaBurstPauseTime = .5f;
    public bool canFire = true;
    private float pDamageModAmt = 1;

    // Audio
    public AudioSource playerAudio;

    public AudioClip plasmaClip;

    void Start()
    { 
        playerRB = GetComponent<Rigidbody>();
        playerHealth = maxHealth;

        levelUpActions = new Action[]
        {
            () => plasmaFireTimes++,
            () => weaponPauseTime *= .9f,
            () => plasmaBurstPauseTime -= .1f,
            () => moveSpeed *= 1.1f,
            () => armorVal += 2,

        };
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
        AsteroidController asteroidController = collision.gameObject.GetComponent<AsteroidController>();

        switch (tag)
        {
            case "Enemy":
                Debug.Log("Enemy Collision");
                DamagePlayer(dmgComponent.GetDamage());
                break;
            
            case "AsteroidSmall":
            case "AsteroidMedium":
            case "AsteroidLarge":
                if (asteroidController != null)
                {
                    float damageAmt = asteroidController.GetAsteroidCollisionDmg();
                    Debug.Log($"Player should take {damageAmt} damage from tag {tag} controller is {asteroidController}");
                    DamagePlayer(damageAmt);
                }
                break;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        EnemyProjectiles enemyProjectile = other.gameObject.GetComponent<EnemyProjectiles>();
        RepairController repairController = other.gameObject.GetComponent<RepairController>();
        AsteroidController asteroidController = other.gameObject.GetComponent<AsteroidController>();

        switch (tag)
        {
            //case "AsteroidSmall":
            //case "AsteroidMedium":
            //case "AsteroidLarge":
            //    float damageAmt = asteroidController.GetAsteroidCollisionDmg();
            //    Debug.Log($"Player should take {damageAmt} damage from tag {tag} controller is {asteroidController}");
            //    DamagePlayer(damageAmt);
            //    break;
            case "PowerUp":
                Debug.Log("PowerUp Collected");
                if (repairController != null)
                {
                    maxHealth += repairController.MaxHealthIncAmount();
                    HealPlayer(repairController.MaxHealthIncAmount());
                }
                //Destroy(other.gameObject);
                break;
            case "Experience":
                Debug.Log("Experience Collected");
                float gatheredExp = 0;
                if (other.name == "BasicExp(Clone)")
                {
                    gatheredExp = 20;
                }
                IncPlayerExp(gatheredExp);
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
            playerHealth -= (amount - armorVal);
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
        GameObject plasmaInstance;
        // choose side to fire from based on in passed in
        playerAudio.PlayOneShot(plasmaClip);
        if (side == 0 || side % 2 == 0)
        {
            
            Vector3 playerRightGun = new Vector3(.7f, .4f, .8f);
            plasmaInstance = Instantiate(plasma, playerRB.transform.position + playerRightGun, transform.rotation);
            PlayerWeaponController pWeapCont =  plasmaInstance.GetComponent<PlayerWeaponController>();
            pWeapCont.SetDamage(pDamageModAmt);
        }
        else
        {
            Vector3 playerLeftGun = new Vector3(-.7f, .4f, .8f);
            plasmaInstance = Instantiate(plasma, playerRB.transform.position + playerLeftGun, transform.rotation);
            PlayerWeaponController pWeapCont = plasmaInstance.GetComponent<PlayerWeaponController>();
            pWeapCont.SetDamage(pDamageModAmt);
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

    public void IncPlayerExp(float amt)
    {
        float incAmount = amt * expModifier;
        playerExp += incAmount;
        Debug.Log("Player got exp " + incAmount + " increasing to " +  playerExp);
        if (playerExp > playerExpToLevel)
        {
            float leftOverExp = playerExp - playerExpToLevel;
            LevelUpPlayer(leftOverExp);
        }
    }

    public void LevelUpPlayer(float bonusExp)
    {
        playerLevel++;
        playerExpToLevel *= expToLevelModifier;
        playerExp = bonusExp;
        Debug.Log("Player Level up to " +  playerLevel + " next xp to lvl " + playerExpToLevel + " curr exp at " + playerExp);
        int idx = UnityEngine.Random.Range(0, levelUpActions.Length);
        levelUpActions[idx].Invoke();
        Debug.Log($"Level {playerLevel} - Action {idx} applied");
        //switch (playerLevel)
        //{
        //    case 1:
        //        plasmaFireTimes++;
        //        break;
        //    case 2:
        //        weaponPauseTime *= .9f;
        //        break;
        //    case 3:
        //        plasmaBurstPauseTime -= .1f;
        //        break;
        //    case 4:
        //        plasmaFireTimes++;
        //        break;
        //    default:
        //        plasmaFireTimes++;
        //        break;
        //}

    }
}
