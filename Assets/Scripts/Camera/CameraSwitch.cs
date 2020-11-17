using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineBrain mainCameraBrain;
    //list of cameras to switch
    public List<CinemachineVirtualCamera> cameras;
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
        if(player.CurrentWaypoint == cameraSwitchWaypoints[0])
        {
            //attiva la camera relativa al punto
            cameras[0].VirtualCameraGameObject.SetActive(true);
            //disattiva la camera precedente se esiste
            if(cameras[1].VirtualCameraGameObject.activeInHierarchy == true)
            {
                cameras[1].VirtualCameraGameObject.SetActive(false);
            }
        }
        if(player.CurrentWaypoint == cameraSwitchWaypoints[1])
        {
            cameras[1].VirtualCameraGameObject.SetActive(true);
            if (cameras[0].VirtualCameraGameObject.activeInHierarchy == true)
            {
                cameras[0].VirtualCameraGameObject.SetActive(false);
            }
        }
    }
}
