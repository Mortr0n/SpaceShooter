using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    public float speed = 10f;
    public float damageAmount;

    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime); 
    }
}
