using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbiting : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector] public Vector3 endRotation;



    [HideInInspector] public float speedRotation = 2f;
    [HideInInspector] public float speedOrbiting = 2f;

    [HideInInspector] public Vector3 perpDirection;

    [HideInInspector] public float position0;



    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(new Vector3(0, 0, 0), perpDirection, (Time.deltaTime) * speedOrbiting);
        transform.Rotate(endRotation * Time.deltaTime / speedRotation);

    }
}
