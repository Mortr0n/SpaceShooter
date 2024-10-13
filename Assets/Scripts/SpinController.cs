using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinController : MonoBehaviour
{
    public float spinSpeed;
    private string tagString;
    // Start is called before the first frame update
    void Start()
    {
        tagString = gameObject.tag;
    }

    // Update is called once per frame
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
