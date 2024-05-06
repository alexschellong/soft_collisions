using System.Collections;
using UnityEngine;

public class TopOfMesh : MonoBehaviour


{


    [SerializeField] private Rigidbody rb;

    public GameObject topVertexPos;
    public GameObject throwVertexPos;
    public GameObject oldEndPosNull;

    private GameObject directionTransform;


    public GameObject camera;
    public GameObject offsetNull;
    public GameObject offsetPointNull;


    public float waitTime = 0.5f;





    float durationOfLerpAdjustNew;
    float durationOfLerpAdjustOld;
    float elapsedTimeAdjust;
    float percentageCompleteAdjust = 0;




    public float forceThrow;
    public float offset = 0;

    // private bool firstRound = true;
    private Vector3 endPos;
    private Vector3 currentPos;

    Vector3 direction;

    bool cameraCaughtUp = false;
    bool waitingFinished = false;


    private float distanceBetweenPointsValue;
    private float distanceBetweenPointsValueOld;
    public float lerpRotateSpeed = 1;


    private float distanceBetween2Points(Vector3 point1, Vector3 point2)
    {

        float deltaX = point1.x - point2.x;
        float deltaY = point1.y - point2.y;
        float deltaZ = point1.z - point2.z;
        float distance = (float)Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

        return distance;
    }


    //private Vector3 find3DpointWithDistance(Vector3 point, float distance, Vector3 direction)
    //{
    //    Debug.Log(direction);
    //    float angleX = Mathf.Deg2Rad * direction.x;
    //    float angleY = Mathf.Deg2Rad * direction.y;

    //    float x = point.x + distance * Mathf.Sin(angleY) * Mathf.Cos(angleX);
    //    float y = point.y + distance * Mathf.Sin(angleX);
    //    float z = point.z+ distance * Mathf.Cos(angleY) * Mathf.Cos(angleX);




    //    return new Vector3(x,y,z);
    //}




    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(waiter());
        currentPos = camera.transform.position;
        // directionTransform = Quaternion.Euler(playerCameraTransform.eulerAngles.x, transform.eulerAngles.y, 0);

        //var x = top.transform.eulerAngles.x;
        // var y = top.transform.eulerAngles.y;
        //   var z = top.transform.eulerAngles.z;
        //  directionTransform = top;

        directionTransform = camera;
        //var x = UnityEditor.TransformUtils.GetInspectorRotation(directionTransform.transform).x;
        //var y = UnityEditor.TransformUtils.GetInspectorRotation(directionTransform.transform).y;
        //var z = UnityEditor.TransformUtils.GetInspectorRotation(directionTransform.transform).z;
        var x = directionTransform.transform.eulerAngles.x;
        var y = directionTransform.transform.eulerAngles.y;
        var z = directionTransform.transform.eulerAngles.z;

        direction = new Vector3(x, y, z);






    }

    IEnumerator waiter()
    {



        yield return new WaitForSeconds(waitTime);

        waitingFinished = true;

    }






    void Update()
    {

        // var x = UnityEditor.TransformUtils.GetInspectorRotation(directionTransform.transform).x;
        // var y = UnityEditor.TransformUtils.GetInspectorRotation(directionTransform.transform).y;
        //   var z = UnityEditor.TransformUtils.GetInspectorRotation(directionTransform.transform).z;

        // Vector3 direction = new Vector3(x, y, z);
        Vector3[] verts = GetComponent<MeshFilter>().sharedMesh.vertices;


        Vector3 directionUp = Quaternion.Euler(direction) * Vector3.up;

        Vector3 worldUp = transform.InverseTransformDirection(directionUp).normalized;
        Plane p = new Plane(worldUp, Vector3.zero);
        Vector3 topVertex = Vector3.zero;
        float maxDist = float.NegativeInfinity;
        for (int i = 0; i < verts.Length; i++)
        {
            float dist = p.GetDistanceToPoint(verts[i]);
            if (dist > maxDist)
            {
                maxDist = dist;
                topVertex = verts[i];
            }
        }
        topVertex = transform.TransformPoint(topVertex);
        topVertexPos.transform.position = topVertex;



        Vector3 directionOfThrow = Quaternion.Euler(direction) * Vector3.back;

        Vector3 worldThrow = transform.InverseTransformDirection(directionOfThrow).normalized;
        Plane p2 = new Plane(worldThrow, Vector3.zero);
        Vector3 throwVertex = Vector3.zero;
        maxDist = float.NegativeInfinity;
        for (int i = 0; i < verts.Length; i++)
        {
            float dist = p2.GetDistanceToPoint(verts[i]);
            if (dist > maxDist)
            {
                maxDist = dist;
                throwVertex = verts[i];
            }
        }
        throwVertex = transform.TransformPoint(throwVertex);
        throwVertexPos.transform.position = throwVertex;



        //if (firstRound)
        //{
        float zVar;
        float xVar;
        float yVar;
        //   firstRound = false;


        //get distance from center of gameobject to throw vertex
        float distanceCenterThrow = Vector3.Distance(throwVertex, transform.position);

        //get a vector3 position by adding distanceCenterThrow to center of the gameobject in the direction of the throw
        Vector3 offsetFromCenterByDistance = transform.position + directionOfThrow * distanceCenterThrow;
        Vector3 offsetFromCenterByDistancePlusOffset = transform.position + directionOfThrow * (distanceCenterThrow + offset);

        //offsetPointNull.transform.position = offsetFromCenterByDistance;

        offsetNull.transform.position = offsetFromCenterByDistancePlusOffset;
        //offsetNull.transform.position = new Vector3(offsetFromCenterByDistance.x, topVertex.y, offsetFromCenterByDistance.z);
        //offsetNull.transform.position += offsetNull.transform.TransformDirection(0,0, -offset);






        //create planes out of points in the direction of the throw and measure distance between them
        Plane p3 = new Plane(worldThrow, throwVertex);

        Plane p4 = new Plane(worldThrow, topVertex);

        float distanceBetweenPlanes = Vector3.Distance(p3.ClosestPointOnPlane(topVertex), p4.ClosestPointOnPlane(topVertex));





        //update values
        if (distanceBetweenPlanes < offset)
        {


            yVar = offsetNull.transform.position.y;
            xVar = offsetNull.transform.position.x;
            zVar = offsetNull.transform.position.z;



        }
        else
        {


            xVar = offsetFromCenterByDistance.x;
            yVar = topVertex.y;
            zVar = offsetFromCenterByDistance.z;


        }


        endPos = new Vector3(xVar, yVar, zVar);

        // }








        if (cameraCaughtUp == false && waitingFinished == true)
        {
            currentPos = camera.transform.position;
            distanceBetweenPointsValue = distanceBetween2Points(currentPos, endPos);




            //did the lerp destination change? if so update time
            durationOfLerpAdjustNew = distanceBetweenPointsValue / forceThrow;
            if (Mathf.Abs(durationOfLerpAdjustOld - durationOfLerpAdjustNew) > 0.1)
            {


                durationOfLerpAdjustOld = durationOfLerpAdjustNew;

                elapsedTimeAdjust = 0;
                percentageCompleteAdjust = 0;
            }


            //lerp code
            if (percentageCompleteAdjust < 1)
            {


                elapsedTimeAdjust += Time.deltaTime;
                percentageCompleteAdjust = elapsedTimeAdjust / durationOfLerpAdjustOld;


                camera.transform.position = Vector3.Lerp(currentPos, endPos, percentageCompleteAdjust);


            }
            else
            {


                //when done lerping update time
                elapsedTimeAdjust = 0;
                percentageCompleteAdjust = 0;
                distanceBetweenPointsValueOld = distanceBetweenPointsValue;
                currentPos = endPos;
                camera.transform.position = currentPos;

                cameraCaughtUp = true;
                camera.transform.parent = transform;

                oldEndPosNull.transform.position = endPos;
                oldEndPosNull.transform.parent = transform;







            };


        }



    }
    private void LateUpdate()
    {


        camera.transform.rotation = Quaternion.Euler(direction);

        if (cameraCaughtUp == true)
        {


            currentPos = camera.transform.position;
            distanceBetweenPointsValue = distanceBetween2Points(endPos, oldEndPosNull.transform.position);
            lerpRotateSpeed = (Mathf.Max(Mathf.Abs(rb.angularVelocity.x), Mathf.Abs(rb.angularVelocity.y)));


            if (distanceBetweenPointsValue > 0.4)
            {
                // Debug.Log(percentageCompleteAdjust);
                // Debug.Log("rotate" + lerpRotateSpeed);
                oldEndPosNull.transform.position = endPos;
                elapsedTimeAdjust = 0;
                distanceBetweenPointsValueOld = distanceBetweenPointsValue;


            }



            elapsedTimeAdjust += Time.deltaTime;
            percentageCompleteAdjust = (elapsedTimeAdjust * (lerpRotateSpeed / 10.0f)) / Mathf.Max(1, distanceBetweenPointsValueOld);




            camera.transform.position = Vector3.Lerp(currentPos, oldEndPosNull.transform.position, percentageCompleteAdjust);


            // camera.transform.position = endPos;




        }
    }
}



