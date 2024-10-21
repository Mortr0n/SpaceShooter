using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CartoonFX;

public class PlayerController : MonoBehaviour
{
    // gameobjects
    private Rigidbody playerRB;
    public GameObject plasma;
    public GameObject powerUpAura;
    public GameObject repairCircle;
    private HealthBarController healthBarController;
    private ExpBarController expBarController;

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
    public AudioClip playerExplode;
    public AudioClip laserHit;
    public AudioClip asteroidCollisionClip;
    public AudioClip enemyShipCollisionClip;
    public AudioClip powerUpCollectClip;
    public AudioClip repairCollectClip;
    public AudioClip playerExpCollectClip;
    public AudioClip playerLevelUpClip;

    // Particles
    public ParticleSystem laserImpactParticle;
    public ParticleSystem repairParticle;
    //public ParticleSystem powerUpAura;

    public void RestartPlayerSettings()
    {
        //Move
        moveSpeed = 8;

        //Weapons
        weaponPauseTime = .2f;
        plasmaFireTimes = 3;
        plasmaBurstPauseTime = .5f;
        pDamageModAmt = 1;

        // player lvl and exp
        playerLevel = 0;
        playerExpToLevel = 100f;
        playerExp = 0;
        expModifier = 1f;
        expToLevelModifier = 1.1f; 

        //player health 
        maxHealth = 100f;
        playerHealth = maxHealth;
        armorVal = 1;

        healthBarController = GetComponent<HealthBarController>();
        healthBarController.UpdateHealthBar();

        Debug.Log($"HealthBarController found: {healthBarController != null}");

    }

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerHealth = maxHealth;

        levelUpActions = new Action[]
        {
            () => plasmaFireTimes++,
            () => weaponPauseTime *= .7f,
            () => plasmaBurstPauseTime *= .8f,
            () => moveSpeed *= 1.1f,
            () => armorVal += 2,

        };

        healthBarController = GetComponent<HealthBarController>();
        healthBarController.UpdateHealthBar();
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

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return playerHealth;
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
                PlayPlayerEnemyCollisionSound();
                DamagePlayer(dmgComponent.GetDamage());
                break;
            
            case "AsteroidSmall":
            case "AsteroidMedium":
            case "AsteroidLarge":
                PlayPlayerAsteroidCollisionSound();
                if (asteroidController != null)
                {
                    float damageAmt = asteroidController.GetAsteroidCollisionDmg();
                    Debug.Log($"Player should take {damageAmt} damage from tag {tag} controller is {asteroidController}");
                    DamagePlayer(damageAmt);
                }
                break;
        }

    }

    IEnumerator RunActiveTimerOnGameObject(float wait, GameObject gameObject)
    {
        gameObject.SetActive(true);

        yield return new WaitForSeconds(wait);

        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        EnemyProjectiles enemyProjectile = other.gameObject.GetComponent<EnemyProjectiles>();
        RepairController repairController = other.gameObject.GetComponent<RepairController>();
        AsteroidController asteroidController = other.gameObject.GetComponent<AsteroidController>();

        switch (tag)
        {   // TODO: Add in Power Bomb Power up
            case "PowerBomb": // I'm not using this at this point... 
                Debug.Log("Collected Power Bomb");
                Destroy(other.gameObject);
                break;

            // TODO: Add in double xp power up
            case "DoubleXP":
                Debug.Log("Collected Double XP");
                Destroy(other.gameObject);
                break;

            case "PowerUp":
                PowerUpCollectSound();
                // This was the dumbest fucking thing I've ever dealt with.  It would just not access the particle system no matter what I did.  I just ended up leaving running at awake or 
                // whatever and then setting it active and not because why the fuck could I play this one the way I play every other fucking particle I've done so far!!!!!?!?!?!?!!?
                powerUpAura.SetActive(true); 
                StartCoroutine(RunActiveTimerOnGameObject(1.5f, powerUpAura));
                  
                if (repairController != null)
                {
                    maxHealth += repairController.MaxHealthIncAmount();
                    HealPlayer(repairController.MaxHealthIncAmount());
                }
                break;

            case "Experience":
                PlayPlayerExpCollectSound();
                float gatheredExp = 0;
                if (other.CompareTag("Experience"))
                {
                    // lookie!  magic number.  should make this be on the experience object and then get the amount
                    // from it and I could change it by the differing obj's and also spawn differing ones on a weighted scale
                    gatheredExp = 20; 
                }
                
                IncPlayerExp(gatheredExp);
                Destroy(other.gameObject);
                break;

            case "Laser":
                Destroy(other.gameObject); 
                break;

            case "EnemyLaser":
                if (enemyProjectile != null)
                {
                    
                    PlayPlayerLaserHitSound(); // set these as their own instanced game objects so that they don't fight with me removing stuff

                    // set laser hit particle effect and play
                    Vector3 hitLocation = other.transform.position;
                    ParticleSystem impactEffect = Instantiate(laserImpactParticle, hitLocation, transform.rotation);
                    impactEffect.Play();

                    // damage the player and destroy the laser object
                    float damageAmount = enemyProjectile.damageAmount;
                    DamagePlayer(damageAmount);
                    Destroy(impactEffect.gameObject, impactEffect.main.duration);
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
                RepairCollectSound();
                if (repairParticle != null)
                {
                    repairParticle.Play();
                }
                else
                {
                    Debug.Log("NO REPAIR!!!");
                    Debug.Log("NO REPAIR!!!");
                }
                if (repairController != null)
                {
                    //TODO: Set the Repair Particle here
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

            PlayerUpdateHealthBar();
        }
    }

    // this was necessar for updating the healthbar after scene load in the game manager so I'm using it for everywhere there's a health change now
    public void PlayerUpdateHealthBar()
    {
        healthBarController = GetComponent<HealthBarController>();
        healthBarController.UpdateHealthBar();
    }

    public void DamagePlayer(float amount)
    {

        if (playerHealth > amount)
        {
            playerHealth -= (amount - armorVal);
            PlayerUpdateHealthBar();
        } else
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        GameManagerController.Instance.GameOver();   
        PlayPlayerExplodeSound();
        Debug.Log("Player Death");
        Destroy(gameObject);
    }

    private void FirePlasmaBall(int side)
    {
        GameObject plasmaInstance;
        // choose side to fire from based on in passed in
        playerAudio.volume = .5f;
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
        //Debug.Log("Player got exp " + incAmount + " increasing to " +  playerExp);
        if (playerExp > playerExpToLevel)
        {
            float leftOverExp = playerExp - playerExpToLevel;
            LevelUpPlayer(leftOverExp);
        }
        expBarController = GetComponent<ExpBarController>();
        expBarController.UpdateExpBar();
    }

    public float GetPlayerLevel()
    {
        return playerLevel;
    }

    public void SetPlayerLevel(float newLevel)
    {
        playerLevel = newLevel;
    }

    public float GetCurrentExp()
    {
        return playerExp;
    }

    public float GetExpToLevel()
    {
        return playerExpToLevel;
    }

    public void LevelUpPlayer(float bonusExp)
    {
        PlayLevelUpSound();
        playerLevel++;
        playerExpToLevel *= expToLevelModifier;
        playerExp = bonusExp;
        //Debug.Log("Player Level up to " + playerLevel + " next xp to lvl " + playerExpToLevel + " curr exp at " + playerExp);
        int idx = UnityEngine.Random.Range(0, levelUpActions.Length);
        levelUpActions[idx].Invoke();
        Debug.Log($"Level {playerLevel} - Action {idx} applied");
        //GameManagerController gameManagerController = GameManagerController.Instance;
        int lvlText = Mathf.FloorToInt(playerLevel);
        GameManagerController.Instance.UpdateLevelText(lvlText.ToString());

        
    }

    // Audio Below
    public void PlayPlayerLaserHitSound()
    {
        GameObject audioObject = new GameObject("PlayerLaserHitAudioTemp");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.volume = .3f;
        audioSource.clip = laserHit;
        audioSource.Play();

        Destroy(audioObject, laserHit.length);
    }

    public void PlayPlayerAsteroidCollisionSound()
    {
        GameObject audioObject = new GameObject("PlayerAsteroidCollAudioTemp");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.volume = .6f;
        audioSource.clip = asteroidCollisionClip;
        audioSource.Play();

        Destroy(audioObject, asteroidCollisionClip.length);
    }

    public void PlayPlayerEnemyCollisionSound()
    {
        GameObject audioObject = new GameObject("PlayerEnemyCollAudioTemp");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.volume = .6f;
        audioSource.clip = enemyShipCollisionClip;
        audioSource.Play();

        Destroy(audioObject, enemyShipCollisionClip.length);
    }

    public void PlayPlayerExpCollectSound()
    {
        GameObject audioObject = new GameObject("PlayerEnemyCollAudioTemp");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.volume = 1f;
        audioSource.clip = playerExpCollectClip;
        audioSource.Play();

        Destroy(audioObject, playerExpCollectClip.length);
    }

    public void PlayPlayerExplodeSound()
    {
        GameObject audioObject = new GameObject("PlayerExplodeAudioTemp");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.volume = .3f;
        audioSource.clip = playerExplode;
        audioSource.Play();

        Destroy(audioObject, playerExplode.length);
    }

    public void RepairCollectSound()
    {
        GameObject audioObject = new GameObject("RepairCollectAudioTemp");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.volume = 1f;
        audioSource.clip = repairCollectClip;
        audioSource.Play();

        Destroy(audioObject, repairCollectClip.length);
    }

    public void PowerUpCollectSound()
    {
        GameObject audioObject = new GameObject("PowerUpAudioTemp");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.volume = 1f;
        audioSource.clip = powerUpCollectClip;
        audioSource.Play();

        Destroy(audioObject, powerUpCollectClip.length);
    }

    public void PlayLevelUpSound()
    {
        GameObject audioObject = new GameObject("PowerUpAudioTemp");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.volume = 1f;
        audioSource.clip = playerLevelUpClip;
        audioSource.Play();

        Destroy(audioObject, playerLevelUpClip.length);
    }
}
