using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlasmaFoward : MonoBehaviour
{
    //private Rigidbody rb;
    public float speed = 2500;
    //private float rotationSpeed = 10;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = new Vector3(0, 0, 1f);
        transform.Translate(forward * speed * Time.deltaTime);
    }
}
