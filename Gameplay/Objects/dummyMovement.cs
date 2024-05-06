using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class dummyMovement : MonoBehaviour
{

    public GameObject dummy;
    public List<GameObject> dummyParts;

    // Start is called before the first frame update
    void Start()
    {


        GetChildRecursive(dummy);


        //get random directional vector


        //apply force to 4 random parts of the dummy in random direction but do not repeat the same parts
        for (int i = 0; i < dummyParts.Count; i++)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            int randomPart = Random.Range(0, dummyParts.Count);
            dummyParts[i].GetComponent<Rigidbody>().AddForce(randomDirection * 10);
            //   Debug.Log(dummyParts[randomPart]);
        }


    }


    private void GetChildRecursive(GameObject obj)
    {
        if (null == obj)
            return;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array


            //check if child has a rigidbody
            if (child.gameObject.GetComponent<Rigidbody>() != null)
            {
                dummyParts.Add(child.gameObject);
            }
            GetChildRecursive(child.gameObject);
        }
    }

}
