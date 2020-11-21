using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMeCamera : MonoBehaviour
{
    Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        transform.position = cam.position;
        transform.rotation = cam.rotation;
    }
}
