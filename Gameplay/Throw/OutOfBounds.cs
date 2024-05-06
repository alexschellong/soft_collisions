using System.Collections;

using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    // Start is called before the first frame update
    public pickUp pickUpScript;
    public Camera playerCamera;

    public GameObject throwCamera;
    public Camera projectionCamera;


    private bool outOfBounds = false;

    bool waitingFinished = false;

    IEnumerator waiter()
    {



        yield return new WaitForSeconds(2);

        waitingFinished = true;

    }



    private void OnTriggerExit(Collider other)
    {




        if (other.gameObject.layer == 6)
        {
            /// Debug.Log("here");



            throwCamera.transform.parent = null;
            TopOfMesh topOfMesh = transform.GetComponent<TopOfMesh>();
            topOfMesh.enabled = false;

            StartCoroutine(waiter());
            pickUpScript.doneFlying = true;

        }




    }



    private void Update()
    {
        if (outOfBounds == true)
        {
            Vector3 viewPos = playerCamera.WorldToViewportPoint(transform.position);


            if (viewPos.x > 1.0 || viewPos.x < 0.0)
            {


                Destroy(gameObject);
            }

            if (viewPos.y > 1.0 || viewPos.y < 0.0)
            {

                Vector3 viewPos2 = projectionCamera.WorldToViewportPoint(transform.position);

                if (viewPos.y > 1.0 || viewPos.y < 0.0)
                {


                    Destroy(gameObject);

                }


            }

        };

        if (waitingFinished)
        {

            playerCamera.transform.LookAt(transform);
            Destroy(throwCamera);
            outOfBounds = true;

        }
    }



}
