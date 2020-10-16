using UnityEngine;

public interface IMovable
{
    Waypoint GetWaypointToMove(Vector3 direction);
    void MoveToWaypoint();
    void CalculateObjectSpeed(Vector3 targetPosition);
    Vector3 CalculateTargetPosition(Waypoint waypointToMove);
}
