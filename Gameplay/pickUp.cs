using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUp : MonoBehaviour
{

    [SerializeField] private Transform playerCameraTransform;

    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;

    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject offsetNull;



    private bool grabbed = false;

    [HideInInspector] public bool doneFlying = false;


    private float rawValue;

    public float throwForce;

    private bool mouseButtonReleased = true;


    private FPSController controls;




    private GameObject grabbedObject;
    private Rigidbody grabbedObjectRB;

    [HideInInspector] bool justPicked = true;

    [HideInInspector] public bool canIPickup = true;





    //camera stuff 

    private void Start()
    {
        rawValue = Mathf.Log(pickupLayerMask, 2);
        controls = GetComponent<FPSController>();
    }

    private void Update()
    {



        if (doneFlying == true)
        {

            controls.enabled = true;
            doneFlying = false;
        };

        if (Input.GetMouseButtonDown(0) && grabbed == false && canIPickup == true)
        {



            //  float pickupDistance = 2f;
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupLayerMask))
            {





                if (raycastHit.transform.gameObject.layer == rawValue)
                {

                    // Debug.Log(rawValue);


                    grabbedObject = raycastHit.transform.gameObject;
                    ;

                    grabbedObjectRB = grabbedObject.GetComponent<Rigidbody>();

                    grabbed = true;

                    mouseButtonReleased = false;

                    grabbedObjectRB.constraints = RigidbodyConstraints.FreezePosition;



                };
            };
        }

        if (Input.GetMouseButtonUp(0) && mouseButtonReleased == false)
        {

            mouseButtonReleased = true;


        }



        if (grabbed == true)
        {



            if (justPicked)
            {

                canIPickup = false;
                justPicked = false;
                grabbedObject.transform.position = objectGrabPointTransform.position;
                grabbedObject.transform.parent = objectGrabPointTransform.transform;

            }




            //grabbedObject.transform.eulerAngles += new Vector3(playerCameraTransform.transform.localEulerAngles.x, transform.localEulerAngles.y, grabbedObject.transform.localEulerAngles.z);
            // grabbedObject.transform.eulerAngles = new Vector3(playerCameraTransform.transform.localEulerAngles.x, transform.localEulerAngles.y, grabbedObject.transform.localEulerAngles.z);

            if (Input.GetMouseButtonDown(0) && mouseButtonReleased == true)
            {
                // Debug.Log("yes");



                objectGrabPointTransform.DetachChildren();

                grabbed = false;
                justPicked = true;

                Vector3 force = objectGrabPointTransform.forward * throwForce;

                grabbedObjectRB.constraints = RigidbodyConstraints.None;

                grabbedObjectRB.AddForce(force, ForceMode.VelocityChange);

                controls.enabled = false;

                camera.GetComponent<AudioListener>().enabled = false;

                GameObject x = Instantiate(camera, grabbedObjectRB.transform.position, Quaternion.Euler(playerCameraTransform.eulerAngles.x, transform.eulerAngles.y, 0));
                GameObject c = Instantiate(camera, grabbedObjectRB.transform.position, Quaternion.Euler(playerCameraTransform.eulerAngles.x, transform.eulerAngles.y, 0));

                GameObject y = Instantiate(offsetNull, grabbedObjectRB.transform.position, Quaternion.Euler(playerCameraTransform.eulerAngles.x, transform.eulerAngles.y, 0));


                //x.transform.SetParent(grabbedObjectRB.transform);

                Camera o = c.GetComponent<Camera>();
                o.targetDisplay = 1;
                Camera h = x.GetComponent<Camera>();
                h.farClipPlane = 1000000000;
                h.nearClipPlane = 0.1f;

                c.transform.parent = x.transform;

                //add camera and null to the topOfMesh script
                TopOfMesh topOfMesh = grabbedObject.GetComponent<TopOfMesh>();
                OutOfBounds outOfBounds = grabbedObject.GetComponent<OutOfBounds>();
                topOfMesh.camera = x;
                outOfBounds.throwCamera = x;
                topOfMesh.enabled = true;
                topOfMesh.offsetNull = y;
                topOfMesh.forceThrow = throwForce;







            }



        }



    }




}
