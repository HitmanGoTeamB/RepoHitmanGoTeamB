using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CameraMovement : MonoBehaviour
{
    public abstract void CalculateCameraMovement();

    public CameraMovement GetActiveCameraMovementComponent()
    {
        List<CameraMovement> CameraMovementComponents;
        List<CameraMovement> CameraMovementComponentsActiveInScene = new List<CameraMovement>();

        CameraMovementComponents = CameraMovement.FindObjectsOfType<CameraMovement>().ToList();

        foreach(CameraMovement cameracomponent in CameraMovementComponents)
        {
            if(cameracomponent.isActiveAndEnabled == true)
            {
                CameraMovementComponentsActiveInScene.Add(cameracomponent);
            }
        }

        return CameraMovementComponentsActiveInScene[0];
    }
}
