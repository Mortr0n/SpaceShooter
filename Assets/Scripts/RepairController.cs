using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairController : MonoBehaviour
{
    public float healAmount;
    private float maxHealthIncAmt = 20;
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
        //Debug.Log("Other guy: " + other.gameObject.tag);
        //Debug.Log("ME: " + gameObject.name);
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
