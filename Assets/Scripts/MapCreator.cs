using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapCreator : MonoBehaviour
{
    [Header("Update")]
    [SerializeField] bool updateCoordinates = false;

    Waypoint[,] waypointsInMap;

    void OnValidate()
    {
        if (updateCoordinates)
        {
            UpdateCoordinates();
        }
    }

    void UpdateCoordinates()
    {
        updateCoordinates = false;

        //find every waypoint in scene
        Waypoint[] waypointsInScene = FindObjectsOfType<Waypoint>();

        //order on x and y
        Waypoint[] waypointsByOrder = waypointsInScene.OrderBy(zAxis => zAxis.transform.position.z).ThenBy(xAxis => xAxis.transform.position.x).ToArray();

        waypointsInMap = new Waypoint[waypointsByOrder.Length, waypointsByOrder.Length];

        int currentZ = Mathf.RoundToInt(waypointsByOrder[0].transform.position.z);
        int x = 0;
        int y = 0;
        for (int i = 0; i < waypointsByOrder.Length; i++)
        {
            Waypoint currentWaypoint = waypointsByOrder[i];

            //if go to next row, reset x and increase y
            if (Mathf.RoundToInt(currentWaypoint.transform.position.z) > currentZ)
            {
                x = 0;
                y++;
                currentZ = Mathf.RoundToInt(currentWaypoint.transform.position.z);
            }

            //put in the array and set coordinates
            waypointsInMap[x, y] = currentWaypoint;
            currentWaypoint.SetCoordinates(x, y);

            x++;
        }
    }
}
