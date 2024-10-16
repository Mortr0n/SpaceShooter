using System.Collections;
using System.Collections.Generic;
using UnityEngine;
aa
public class AsteroidController : MonoBehaviour
{
    [SerializeField]
    private float AstHealth;
    private float AstCollisionDmg;


    public float GetAsteroidHealth()
    {
        return AstHealth;
    }

    public float GetAsteroidCollisionDmg() { return AstCollisionDmg; }

    public void SetAsteroidHealth(float amt) 
    {
        AstHealth -= amt;
    }

    public bool DamageAsteroid(float amt)
    {
        SetAsteroidHealth(amt);
        if (AstHealth < 0)
        {
            return true;
        }
        return false;
    }

    void Start()
    {
        switch (gameObject.tag)
        {
            case "AsteroidLarge":
                AstHealth = 85;
                AstCollisionDmg = 40;
                break;
            case "AsteroidMedium":
                AstHealth = 60;
                AstCollisionDmg = 25;
                break;
            case "AsteroidSmall":
                AstHealth = 30;
                AstCollisionDmg = 10;
                break;
            default:
                Debug.LogWarning("Object is not an asteroid!");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
