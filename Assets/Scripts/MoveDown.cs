using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10f;
    public float torqueAmt = 10f;
    public float damageAmount;

    void Start()
    {
        string tag = gameObject.tag;
        //Debug.Log("Tag: " + tag);

        rb = GetComponent<Rigidbody>();
        // add spin to asteroids
        if (CompareTag("Asteroid"))
        {
            rb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        }
        
    }

    private void FixedUpdate()
    {
        // Normal asteroid and other movement
        Vector3 down = new Vector3(0, 0, -1);
        if (rb != null)
        {
            //rb.velocity = down * speed * Time.deltaTime;
            Vector3 downForce = new Vector3(0, 0, -1) * 2f * Time.fixedDeltaTime;
            rb.AddForce(downForce, ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void Update()
    {
       

        // Power up movement in world
        if (CompareTag("Repair") || CompareTag("Experience"))
        {
            //Debug.Log("Inside the repair if ");
            Vector3 moveDown = new Vector3(0, 0, -1) * speed * Time.deltaTime;
            transform.Translate(moveDown, Space.World);
        }
        if (CompareTag("PowerUp"))
        {
            //Debug.Log("Inside the repair if ");
            Vector3 moveDown = new Vector3(0, 0, -1) * speed * Time.deltaTime;
            transform.Translate(moveDown, Space.World);
        }
    }
     
    public float GetDamage()
    {
        return damageAmount;
    }

    float RandomTorque()
    {
        return Random.Range(-torqueAmt, torqueAmt);
    }
}
