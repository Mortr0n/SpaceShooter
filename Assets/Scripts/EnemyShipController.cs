using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    public GameObject laser;
    public GameObject[] expGems;

    // Enemy Weapons
    private float laserTimer = 2f;
    private float largeShipLaserZ = -1.5f;
    private float largeShipAltLaserX = -1.5f;

    // Enemy Stats
    public float shipMaxHealth;
    public float damageAmount; //??

    private float healthModifier = 1;
    private float armorModifier = 0;
    private float shipHealth;

    private int expBase = 0;
    public int enemyTypeRange;

    void Start()
    {
        StartCoroutine(FireLaser());
        shipHealth = shipMaxHealth * healthModifier; 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShipHealth(float health)
    {
        // FIXME!!!
        shipHealth = health;
    }

    public float GetShipHealth() {
        return shipHealth; }

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            // call players weapons destroy script maybe instead of just destroying it?
            Destroy(other.gameObject);
            PlayerWeaponController pWeapCont = other.GetComponent<PlayerWeaponController>();
            Debug.Log(other.tag + " Weap Dmg " +  pWeapCont.GetDamage());

            if (pWeapCont != null)
            {
                DamageEnemy(pWeapCont.GetDamage());
            }
            else
            {
                Debug.LogError("PlayerWeaponController not found on collided player weapon!");
            }
        }
               
        
        if (!other.CompareTag("EnemyLaser") && !other.CompareTag("Repair") && !other.CompareTag("PowerUp") && !other.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject);
            //Destroy(gameObject);
        }
    }

    private void DamageEnemy(float damage)
    {
        float totalDamage = damage - armorModifier;
        Debug.Log("Damaging Enemy for " +  totalDamage);
        if (shipHealth > totalDamage)
        {
            shipHealth -= totalDamage;
            Debug.Log("Enemy " + gameObject.name + " damaged for " + totalDamage + " health is now " + shipHealth);
        }
        else if (shipHealth - totalDamage <= 0)
        {
            {
                Debug.Log("Enemy " + gameObject.name + " health is now " + shipHealth + " and is dying.");
                KillEnemy();
            }
        }
    }

    private void KillEnemy()
    {
        int idx = Random.Range(expBase, enemyTypeRange);
        GameObject expGem = Instantiate(expGems[idx], transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
