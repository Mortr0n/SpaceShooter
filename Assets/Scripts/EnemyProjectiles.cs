using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    public float speed = 10f;
    //private float rotationSpeed = 10;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 forward = new Vector3(0, 0, -1f);
        transform.Translate(transform.forward * speed * Time.deltaTime); 
    }
}
