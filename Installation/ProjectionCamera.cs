using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionCamera : MonoBehaviour
{

    [SerializeField] private Transform cameraPlayer;
    public pickUp pickUpScript;
    Camera camProjection;

    bool firstRound = true;


    private void Start()
    {
        camProjection = GetComponent<Camera>();



    }
    private void Update()
    {
        if (pickUpScript.doneFlying == false)
        {
            if (firstRound)
            {
                firstRound = false;
                camProjection.enabled = true;

            }
            this.transform.position = cameraPlayer.transform.position;
            this.transform.eulerAngles = new Vector3(0, cameraPlayer.transform.eulerAngles.y, 0);
        }
        else if (firstRound == false)
        {

            firstRound = true;
            camProjection.enabled = false;








        }
    }




}
