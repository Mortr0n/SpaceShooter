using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        string taggy = other.tag;
        if (taggy != "EnemyLaser" && taggy != "Repair" && taggy != "PowerUp")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
    }
}
