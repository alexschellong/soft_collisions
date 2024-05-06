using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingObjects : MonoBehaviour
{
    public GameObject Player;

    [SerializeField] private GameObject outerBound;
    [SerializeField] private GameObject innerBound;
    private float outerBoundValue;
    private float innerBoundValue;

    [SerializeField] private GameObject[] prefabs;

    public float speedRotation = 2f;

    public float speedOrbiting;
    public float speedOrbitingMin = 3f;
    public float speedOrbitingMax = 14f;
    public GameObject objectToMorphInto;




    // Start is called before the first frame update
    void Start()
    {
        outerBoundValue = outerBound.transform.localScale.x;
        innerBoundValue = innerBound.transform.localScale.x;



        for (int i = 0; i < prefabs.Length; i++)
        {
            float position0 = Random.Range(innerBoundValue, outerBoundValue);
            Vector3 position1 = Random.onUnitSphere * position0;


            Quaternion startRotation = Quaternion.Euler(Random.Range(0, 360),
                                              Random.Range(0, 360),
                                               Random.Range(0, 360)
                                              );


            Vector3 endRotation = new Vector3(Random.Range(0, 360),
                                                 Random.Range(0, 360),
                                                  Random.Range(0, 360)
                                                 );


            Vector3 direction = (position1 - new Vector3(0, 0, 0)).normalized;
            float angle = Random.Range(0, Mathf.PI * 2f);
            Vector3 inPlane = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            Vector3 perpDirection = Quaternion.LookRotation(direction) * inPlane;

            speedOrbiting = Random.Range(speedOrbitingMin, speedOrbitingMax);



            GameObject x = Instantiate(prefabs[i], position1, startRotation);

            orbiting y = x.AddComponent<orbiting>();
            CollisionMorph c = x.AddComponent<CollisionMorph>();

            y.endRotation = endRotation;
            y.speedRotation = speedRotation;
            y.speedOrbiting = speedOrbiting;
            y.perpDirection = perpDirection;
            y.position0 = position0;

            c.Player = Player;
            c.objectToMorphInto = objectToMorphInto;

        }


    }

}
