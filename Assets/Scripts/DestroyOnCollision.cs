using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public GameObject[] expGems;
    public GameObject smallAsteroid;
    public GameObject medAsteroid;
    public GameObject thisObject;

    private int expBase = 0;
    public int enemyTypeRange;

    // Audio
    public AudioSource asteroidAudioSource;
    public AudioClip asteroidLaserHitClip;

    // Particles
    public ParticleSystem plasmaHitParticle;
    public ParticleSystem astSmokeParticle;

    public void PlayAsteroidHitSound()
    {
        GameObject audioObject = new GameObject("AsteroidHitAudioTemp");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.volume = .3f;
        audioSource.clip = asteroidLaserHitClip;
        audioSource.Play();

        Destroy(audioObject, asteroidLaserHitClip.length);
    }
    void OnTriggerEnter(Collider other)
    {
        Vector3 hitLocation = other.transform.position;
        ParticleSystem plasmaImpactParticle = Instantiate(plasmaHitParticle, hitLocation, transform.rotation);
        //if (tag != "EnemyLaser" && tag != "Repair" && tag != "PowerUp")
        if (other.CompareTag("PlayerWeapon") || other.CompareTag("Player"))
        {
            PlayAsteroidHitSound();
            AsteroidController aController = gameObject.GetComponent<AsteroidController>();

            string objectTag = gameObject.tag;
            int idx = Random.Range(expBase, enemyTypeRange);
            
            float damageAmount = 0;
            if (other.CompareTag("PlayerWeapon"))
            {
                plasmaImpactParticle.Play();
                PlayerWeaponController pWeap = other.GetComponent<PlayerWeaponController>();
                damageAmount = pWeap.GetDamage();
            }
           
            Destroy(other.gameObject);
            Destroy(plasmaImpactParticle.gameObject, plasmaImpactParticle.main.duration);
           
            
            Vector3 offset1 = new Vector3(1, 0, 1);
            Vector3 offset2 = new Vector3(-1, 0, -1);

            
            switch (objectTag)
            {
                case "AsteroidLarge":
                    
                    if (aController != null) 
                    {
                        
                        if (aController.DamageAsteroid(damageAmount)) // returns true if asteroid health <= 0
                        {
                            // Generate 2 new asteroids at explosioin
                            GameObject medRock1 = Instantiate(medAsteroid, transform.position + offset1, transform.rotation);
                            GameObject smallRock2 = Instantiate(smallAsteroid, transform.position + offset2, transform.rotation);
                            
                            // Apply force to blow them apart
                            Rigidbody rb1 = medRock1.GetComponent<Rigidbody>();
                            Rigidbody rb2 = smallRock2.GetComponent<Rigidbody>();
                            rb1.AddForce(new Vector3(1, 0, 1) * 3f, ForceMode.Impulse);  // Push in random directions
                            rb2.AddForce(new Vector3(-1, 0, -1) * 3f, ForceMode.Impulse);
                            
                            // make exp gem and particle system and get shake effect component from the particle system
                            GameObject expGem = Instantiate(expGems[idx], transform.position, transform.rotation);
                            ParticleSystem astGoBoom = Instantiate(astSmokeParticle, transform.position, transform.rotation);
                            CFXR_Effect shakeEffect = astGoBoom.GetComponent<CFXR_Effect>();

                            // no shake and play boom an clean it all up
                            if (shakeEffect != null) { shakeEffect.enabled = false; }
                            astGoBoom.Play();
                            Destroy(gameObject);
                            Destroy(astGoBoom.gameObject, astGoBoom.main.duration);
                        }
                    }
                    break;

                case "AsteroidMedium":
                    //AsteroidController aController = gameObject.GetComponent<AsteroidController>();
                    if (aController != null)
                    {
                        if (aController.DamageAsteroid(damageAmount)) // returns true if asteroid health <= 0
                        {
                            // make 2 new small asteroids at this location
                            GameObject smallRock3 = Instantiate(smallAsteroid, transform.position + offset1, transform.rotation);
                            GameObject smallRock4 = Instantiate(smallAsteroid, transform.position + offset2, transform.rotation);
                            
                            // Apply force to blow them apart
                            Rigidbody rb3 = smallRock3.GetComponent<Rigidbody>();
                            Rigidbody rb4 = smallRock4.GetComponent<Rigidbody>();
                            rb3.AddForce(new Vector3(1, 0, -1) * 3f, ForceMode.Impulse);
                            rb4.AddForce(new Vector3(-1, 0, 1) * 3f, ForceMode.Impulse);

                            // drop exp Gem
                            GameObject expGem = Instantiate(expGems[idx], transform.position, transform.rotation);
                            
                            // make smoke from explosion object at location of death
                            ParticleSystem astGoBoom = Instantiate(astSmokeParticle, transform.position, transform.rotation);
                            
                            // stop camera shake
                            CFXR_Effect shakeEffect = astGoBoom.GetComponent<CFXR_Effect>();
                            if (shakeEffect != null) { shakeEffect.enabled = false; }
                            
                            // play explosion
                            astGoBoom.Play();

                            // Clean up everything 
                            Destroy(gameObject);
                            Destroy(astGoBoom.gameObject, astGoBoom.main.duration);
                        }
                    }
                    
                    break;
                case "AsteroidSmall":
                    if (aController != null)
                    {
                        if (aController.DamageAsteroid(damageAmount))
                        {
                            // Get all components for making the asteroid blow up
                            GameObject expGem = Instantiate(expGems[idx], transform.position, transform.rotation);
                            ParticleSystem astGoBoom = Instantiate(astSmokeParticle, transform.position, transform.rotation);
                            CFXR_Effect shakeEffect = astGoBoom.GetComponent <CFXR_Effect>();

                            // no shake
                            if (shakeEffect != null) { shakeEffect.enabled = false; }
                            
                            // explosion effect and clean everything up
                            astGoBoom.Play();
                            Destroy(gameObject);
                            Destroy(astGoBoom.gameObject, astGoBoom.main.duration);
                        }
                    }
                    break;
                default:
                    //Destroy(gameObject);
                    Debug.LogError($"Tag: {objectTag} not covered in DestroyOnCollision controller ");
                    break;
            }

            

        }
    }
}
