using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairController : MonoBehaviour
{
    public float healAmount;
    private float maxHealthIncAmt = 20;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            Destroy(gameObject);
        }
    }

    public float MaxHealthIncAmount()
    {
        return maxHealthIncAmt;
    }
}
