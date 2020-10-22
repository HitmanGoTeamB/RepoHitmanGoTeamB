using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[SelectionBase]
public class Character : StateMachine, IMovable
{
    #region variables

    [Tooltip("Time to move from one waypoint to another")]
    [SerializeField] float timeToMove = 1.5f;

    Waypoint currentWaypoint;
    protected Waypoint CurrentWaypoint
    {
        get
        {
            //if not set, get current waypoint
            if (currentWaypoint == null)
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

    #endregion

    void Start()
    {
        //add on this waypoint
        CurrentWaypoint.AddObjectToWaypoint(this.gameObject);
    }

    #region movement

    public IEnumerator Move(Waypoint waypointToReach)
    {
        //save start position
        Vector3 startPosition = transform.position;

        //movement
        float delta = 0;
        while(delta < 1)
        {
            delta += Time.deltaTime / timeToMove;

            transform.position = Vector3.Lerp(startPosition, targetPosition, delta);

            yield return null;
        }

        //set final position and remove from waypoint
        transform.position = targetPosition;
        CurrentWaypoint.RemoveObjectFromWaypoint(this.gameObject);

        //set new current waypoint and add to this waypoint
        currentWaypoint = waypointToReach;
        CurrentWaypoint.AddObjectToWaypoint(this.gameObject);

        //go to wait state after finish the movement
        SetState(new Wait(this));

    }

    public void SetTargetPosition(Waypoint waypointToReach)
    {
        //get target position (waypoint X and Z axis, but character Y axis)
        targetPosition = new Vector3(waypointToReach.transform.position.x, transform.position.y, waypointToReach.transform.position.z);
    }

    public Waypoint GetWaypointToMove(Vector2Int direction)
    {
        //get waypoint in that direction
        Waypoint waypointToReach = GameManager.instance.map.GetWaypointInDirection(CurrentWaypoint, direction);

        //if there is a waypoint, check if is walkable and return
        if(waypointToReach != null)
        {
            if(CurrentWaypoint.WalkableWaypoints.Contains(waypointToReach))
            {
                return waypointToReach;
            }
        }

        return null;
    }

    #endregion

    void GetCurrentWaypoint()
    {
        //find current waypoint with a raycast to the down
        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down, out hit);
        currentWaypoint = hit.transform.gameObject.GetComponent<Waypoint>();
    }
}
