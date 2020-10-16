using UnityEngine;

public interface IMovable
{
    Waypoint GetWaypointToMove(Vector3 direction);
    void CalculateMovement(Waypoint waypointToReach);
    void Movement();
}
