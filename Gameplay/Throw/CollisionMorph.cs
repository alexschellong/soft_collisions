using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionMorph : MonoBehaviour
{
    public GameObject objectToMorphInto;
    public GameObject Player;
    public pickUp pickUpScript;


    private void Start()
    {
        pickUpScript = Player.GetComponent<pickUp>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {

            Vector3 objectPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Quaternion randomRotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
            GameObject x = Instantiate(objectToMorphInto, objectPosition, randomRotation) as GameObject;
            pickUpScript.doneFlying = true;
            pickUpScript.canIPickup = true;

            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
    }
}
