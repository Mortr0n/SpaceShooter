using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public GameObject[] expGems;
    public GameObject smallAsteroid;
    public GameObject medAsteroid;

    private int expBase = 0;
    public int enemyTypeRange;
    void OnTriggerEnter(Collider other)
    {
        if (tag != "EnemyLaser" && tag != "Repair" && tag != "PowerUp")
        {
            string objectName = gameObject.name;
            int idx = Random.Range(expBase, enemyTypeRange);
            GameObject expGem = Instantiate(expGems[idx], transform.position, transform.rotation);
            
            Destroy(other.gameObject);
            
            Debug.Log("objectName: " + objectName); 
            switch (objectName)
            {
                case "asteroidLarge_1(Clone)":
                    GameObject medRock1 = Instantiate(medAsteroid, transform.position, transform.rotation); 
                    GameObject smallRock2 = Instantiate(smallAsteroid, transform.position, transform.rotation);
                    // Apply force to blow them apart
                    Rigidbody rb1 = medRock1.GetComponent<Rigidbody>();
                    Rigidbody rb2 = smallRock2.GetComponent<Rigidbody>();
                    rb1.AddForce(new Vector3(1, 0, 1) * 5f, ForceMode.Impulse);  // Push in random directions
                    rb2.AddForce(new Vector3(-1, 0, -1) * 5f, ForceMode.Impulse);
                    break;
                case "asteroidMed_2(Clone)":
                    GameObject smallRock3 = Instantiate(smallAsteroid, transform.position, transform.rotation);
                    GameObject smallRock4 = Instantiate(smallAsteroid, transform.position, transform.rotation);
                    // Apply force to blow them apart
                    Rigidbody rb3 = smallRock3.GetComponent<Rigidbody>();
                    Rigidbody rb4 = smallRock4.GetComponent<Rigidbody>();
                    rb3.AddForce(new Vector3(1, 0, -1) * 5f, ForceMode.Impulse);
                    rb4.AddForce(new Vector3(-1, 0, 1) * 5f, ForceMode.Impulse);
                    break;
                default:
                    break;
            }

            Destroy(gameObject, 0.1f);

        }
    }
}
