using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Hitman GO/Managers/UI Manager")]
public class UIManager : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float height = 2;

    [Header("Rock")]
    [SerializeField] Canvas rockPoint = default;
    [SerializeField] Canvas rockArea = default;

    Pooling<Canvas> rockPoints = new Pooling<Canvas>();

    public void ShowRockPoints(Dictionary<Vector2Int, Waypoint> waypoints)
    {
        //foreach waypoint
        foreach(Waypoint waypoint in waypoints.Values)
        {
            if (waypoint != null)
            {
                Vector3 position = waypoint.transform.position + Vector3.up * height;

                //pooling instantiate rock point, and set parent and position
                Canvas point = rockPoints.Instantiate(rockPoint, transform);
                point.transform.position = position;
            }
        }
    }

    public void HideRockPoints()
    {
        rockPoints.DeactiveAll();
    }
}
