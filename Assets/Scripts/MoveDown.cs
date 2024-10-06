using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    public float torqueAmt = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (CompareTag("Asteroid"))
        {
            rb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 down = new Vector3(0, 0, -1);
        rb.velocity = down * speed * Time.deltaTime;

    }

    float RandomTorque()
    {
        return Random.Range(-torqueAmt, torqueAmt);
    }
}
