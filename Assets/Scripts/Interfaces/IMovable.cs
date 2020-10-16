using UnityEngine;
using System.Collections;

public interface IMovable
{
    Waypoint GetWaypointToMove(Vector2Int direction);
    void SetTargetPosition(Waypoint waypointToReach);
    IEnumerator Move(Waypoint waypointToReach);
}
