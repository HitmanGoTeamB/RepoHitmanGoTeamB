using UnityEngine;
using System.Collections;

public interface IMovable
{
    Waypoint GetWaypointToMove(Waypoint waypointToReach, bool getEveryWaypoint);
    void SetTargetPosition(Waypoint waypointToReach);
    IEnumerator Move(Waypoint waypointToReach);
}
