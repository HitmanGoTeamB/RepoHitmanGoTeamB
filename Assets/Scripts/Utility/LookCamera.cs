using UnityEngine;

[AddComponentMenu("Hitman GO/Utility/Look Camera")]
public class LookCamera : MonoBehaviour
{
    [Header("Important")]
    [Tooltip("Don't follow y axis of the camera (up or down)")]
    [SerializeField] bool ignoreYAxis = true;
    [SerializeField] bool rotateOnlyYAxis = true;

    [Header("Override, if you don't want to use defaults")]
    [Tooltip("Default is main camera")]
    [SerializeField] Camera cam = default;
    [Tooltip("Default is this transform")]
    [SerializeField] Transform transformToRotate = default;
    [SerializeField] Transform otherTransformToRotate = default;

    void Start()
    {
        //get main camera
        if (cam == null)
            cam = Camera.main;

        //get this gameObject
        if (transformToRotate == null)
            transformToRotate = transform;
    }

    void Update()
    {
        if (cam && transformToRotate)
        {
            //look at camera
            Vector3 cameraPosition = cam.transform.position;

            //ignore y axis
            if (ignoreYAxis)
                cameraPosition.y = transformToRotate.position.y;

            //get look rotation
            Vector3 direction = cameraPosition - transformToRotate.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            //set rotation
            SetRotation(transformToRotate, rotation);

            //set other transform rotation
            if (otherTransformToRotate)
                SetRotation(otherTransformToRotate, rotation);
        }
    }

    void SetRotation(Transform objectToRotate, Quaternion rotation)
    {
        //set rotation
        if (rotateOnlyYAxis)
            objectToRotate.eulerAngles = new Vector3(objectToRotate.eulerAngles.x, rotation.eulerAngles.y, objectToRotate.eulerAngles.z);
        else
            objectToRotate.rotation = rotation;
    }
}