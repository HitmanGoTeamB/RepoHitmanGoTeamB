using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineBrain mainCameraBrain;
    //default camera in scene
    public CinemachineVirtualCamera defaultCamera;
    //camera to switch to from the default one
    public CinemachineVirtualCamera switchCamera;
    //list of tile positions where the camera switch
    public List<Waypoint> cameraSwitchWaypoints; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SwitchCameraPosition();
    }

    void SwitchCameraPosition()
    {
        Player player = GameManager.instance.player;

        if (cameraSwitchWaypoints.Contains(player.CurrentWaypoint))
        {
            switchCamera.VirtualCameraGameObject.SetActive(true);
            defaultCamera.VirtualCameraGameObject.SetActive(false);
        }
        else
        {
            defaultCamera.VirtualCameraGameObject.SetActive(true);
            switchCamera.VirtualCameraGameObject.SetActive(false);
        }
    }
}
