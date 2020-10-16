using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character : MonoBehaviour
{
    [SerializeField] float timeToMove = 1.5f;
    Waypoint currentWaypoint;

    Waypoint CurrentWaypoint
    {
        get
        {
            if(currentWaypoint == null)
            {
                GetCurrentWaypoint();
            }
            return currentWaypoint;
        }
        set
        {
            currentWaypoint = value;
        }
    }

    protected void MoveToWaypoint(Vector3 directionToMove)
    {
        Waypoint waypointToMove = GetWaypointToMove(directionToMove);
        Vector3 targetPosition = new Vector3(waypointToMove.transform.position.x, transform.position.y, waypointToMove.transform.position.z);
        float SpeedToMove = Vector3.Distance(transform.position, targetPosition) / timeToMove;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, SpeedToMove);
    }

    Waypoint GetWaypointToMove(Vector3 direction)
    {
        Waypoint waypointToMove = null;
        CurrentWaypoint.WalkableWaypoints.OrderBy(distance => Vector3.Distance(distance.transform.position, CurrentWaypoint.transform.position + direction));
        if(CurrentWaypoint.WalkableWaypoints[0] != null)
        {
            waypointToMove = CurrentWaypoint.WalkableWaypoints[0];           
        }
        return waypointToMove;
    }

    void GetCurrentWaypoint()
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down, out hit);
        currentWaypoint = hit.transform.gameObject.GetComponent<Waypoint>();
    }
}
