using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character : StateMachine, IMovable
{
    [Tooltip("Time to move from one waypoint to another")]
    [SerializeField] float timeToMove = 2;

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

    Vector3 targetPosition;
    float speedToMove;

    public void MoveToWaypoint()
    {
        //move
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedToMove * Time.deltaTime);
    }

    public Waypoint GetWaypointToMove(Vector3 direction)
    {
        Waypoint waypointToMove = null;

        //get nearest waypoint in that direction
        CurrentWaypoint.WalkableWaypoints.OrderBy(distance => Vector3.Distance(distance.transform.position, CurrentWaypoint.transform.position + direction));
        if(CurrentWaypoint.WalkableWaypoints[0] != null)
        {
            waypointToMove = CurrentWaypoint.WalkableWaypoints[0];           
        }

        return waypointToMove;
    }

    void GetCurrentWaypoint()
    {
        //find current waypoint with a raycast to the down
        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down, out hit);
        currentWaypoint = hit.transform.gameObject.GetComponent<Waypoint>();
    }

    public void CalculateObjectSpeed(Vector3 targetPosition)
    {
        speedToMove = Vector3.Distance(transform.position, targetPosition) / timeToMove;
    }

    public Vector3 CalculateTargetPosition(Waypoint waypointToMove)
    {
        //get target position (waypoint on X and Z axis, but player Y axis)
        targetPosition = new Vector3(waypointToMove.transform.position.x, transform.position.y, waypointToMove.transform.position.z);
        return targetPosition;
    }
}
