using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlasmaFoward : MonoBehaviour
{
    public float speed = 2500;

    void Update()
    {
        Vector3 forward = new Vector3(0, 0, 1f);
        transform.Translate(forward * speed * Time.deltaTime);
    }
}
