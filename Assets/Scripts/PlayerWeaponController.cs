using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public float initDamageAmount;
    private float damageAmount;
    private float initDamageModifier = 1;
    // Start is called before the first frame update
    void Start()
    {
        SetDamage(initDamageModifier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetDamage(float damageModifier)
    {
        float damage = initDamageAmount * damageModifier; // ability to change how much damage is output using modifier
        damageAmount = damage;
    }
    public float GetDamage()
    {
        Debug.Log("Getting Damage " + damageAmount);
        return damageAmount;
    }
}
