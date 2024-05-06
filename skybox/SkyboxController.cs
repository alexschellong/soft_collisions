using UnityEngine;

[ExecuteAlways]
public class SkyboxController : MonoBehaviour
{
    public Transform _Sun = default;

    void LateUpdate()
    {
        // Directions are defined to point towards the object

        // Sun
        Shader.SetGlobalVector("_SunDir", -_Sun.transform.forward);


    }
}