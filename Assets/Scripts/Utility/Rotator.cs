using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Hitman GO/Utility/Rotator")]
public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 axisToRotate = Vector3.up;
    [SerializeField] float speed = 4;

    void Update()
    {
        //rotate on axis
        transform.Rotate(axisToRotate * speed * Time.deltaTime);
    }
}
