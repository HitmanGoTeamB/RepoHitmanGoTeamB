using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[SelectionBase]
public class Character : StateMachine, IMovable
{
    #region variables

    [Header("Time animation movement")]
    [SerializeField] float timeToMove = 1.5f;

    Waypoint currentWaypoint;
    public Waypoint CurrentWaypoint
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

    //position to reach (over waypoint)
    Vector3 targetPosition;

    Vector2Int[] fourDirectionsVectors = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

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

    public Waypoint GetWaypointToMove(Waypoint waypointToReach, bool getEveryWaypoint)
    {
        //if there is a waypoint, check if is walkable and return
        if(waypointToReach != null)
        {
            if(getEveryWaypoint || CurrentWaypoint.WalkableWaypoints.Contains(waypointToReach))
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
        int layer = CreateLayer.LayerAllExcept("Player");                               //use layer to ignore Player layer
        Physics.Raycast(this.transform.position, Vector3.down, out hit, 10, layer);
        currentWaypoint = hit.transform.gameObject.GetComponent<Waypoint>();
    }

    #region public API

    public List<Waypoint> GetAllWaypointsAroundMe(Waypoint currentWaypoint)
    {
        List<Waypoint> allWaypointsAround = new List<Waypoint>();

        //foreach direction, add waypoint to the list
        foreach (Vector2Int direction in fourDirectionsVectors)
        {
            allWaypointsAround.Add(GameManager.instance.map.GetWaypointInDirection(currentWaypoint, direction));
        }

        return allWaypointsAround;
    }

    public void GetAllWaypointsAroundMe(Dictionary<Vector2Int, Waypoint> dictionary)
    {
        dictionary.Clear();

        //fill dictionary for every direction
        foreach (Vector2Int direction in fourDirectionsVectors)
        {
            dictionary.Add(direction, GameManager.instance.map.GetWaypointInDirection(CurrentWaypoint, direction));
        }
    }

    #endregion
}
