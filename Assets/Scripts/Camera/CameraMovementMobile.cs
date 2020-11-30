﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementMobile : CameraMovement
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private CinemachineBrain mainCameraBrain;
    [SerializeField]
    private Transform cameraLookAtPoint;
    private Vector3 previousPosition;
    private ICinemachineCamera currentCamera;
    private Vector3 currentCameraStartingPosition;
    private Quaternion currentCameraStartingRotation;
    private float distanceFromCenter;
    [SerializeField]
    private Transform mapCenter;
    CameraSwitch cameraswitch;

    // Start is called before the first frame update
    void Start()
    {
        cameraswitch = FindObjectOfType<CameraSwitch>();
        currentCamera = mainCameraBrain.ActiveVirtualCamera;
        distanceFromCenter = Vector3.Distance(currentCamera.VirtualCameraGameObject.transform.position, mapCenter.position);
        currentCameraStartingPosition = currentCamera.VirtualCameraGameObject.transform.position;
        currentCameraStartingRotation = currentCamera.VirtualCameraGameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateCameraMovement();
    }

    public override void CalculateCameraMovement()
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            previousPosition = mainCamera.ScreenToViewportPoint(touch.position);
        }
        if (touch.phase == TouchPhase.Moved)
        {
            if (currentCamera != mainCameraBrain.ActiveVirtualCamera)
            {
                currentCamera = mainCameraBrain.ActiveVirtualCamera;
                distanceFromCenter = Vector3.Distance(currentCamera.VirtualCameraGameObject.transform.position, mapCenter.position);
                currentCameraStartingPosition = currentCamera.VirtualCameraGameObject.transform.position;
                currentCameraStartingRotation = currentCamera.VirtualCameraGameObject.transform.rotation;
            }

            Vector3 direction = previousPosition - mainCamera.ScreenToViewportPoint(touch.position);

            //scambiare con la virtual camera da qui in poi
            mainCameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform.position = cameraLookAtPoint.position;

            mainCameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform.Rotate(new Vector3(1, 0, 0), direction.y * 35);

            mainCameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 60, Space.World);

            mainCameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform.Translate(0, 0, -distanceFromCenter);

            previousPosition = mainCamera.ScreenToViewportPoint(touch.position);
        }
        if (touch.phase == TouchPhase.Ended)
        {
            mainCameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform.position = currentCameraStartingPosition;
            mainCameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform.rotation = currentCameraStartingRotation;
        }
    }
}
