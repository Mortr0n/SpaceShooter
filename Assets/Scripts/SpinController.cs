using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinController : MonoBehaviour
{
    public float spinSpeed;
    private string tagString;

    void Start()
    {
        tagString = gameObject.tag;
    }


    void Update()
    {
        if (tagString == "PowerUp")
        {
            transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
        } 
        else
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        }
    }
        
}
