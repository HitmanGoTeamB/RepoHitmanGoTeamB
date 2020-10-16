using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapCreator : MonoBehaviour
{
    [Header("Update")]
    [SerializeField] bool updateCoordinates = false;

    Dictionary<Vector2Int, Waypoint> waypointsInMap = new Dictionary<Vector2Int, Waypoint>();

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

        //reset map
        waypointsInMap.Clear();

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

            //put in the dictionary and set coordinates
            waypointsInMap.Add(new Vector2Int(x, y), currentWaypoint);
            currentWaypoint.SetCoordinates(x, y);

            x++;
        }
    }
}
