using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    public float destroyPos = -24;
    
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyPos < 0)
        {
            if (transform.position.z < destroyPos)
            {
                Destroy(gameObject);
            }
        }
        if (destroyPos > 0)
        {
            if (transform.position.z > destroyPos)
            {
                Destroy(gameObject);
            }


        }
    }
}
