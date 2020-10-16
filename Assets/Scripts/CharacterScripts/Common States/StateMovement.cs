using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMovement : State
{
    IMovable objectToMove;
    Waypoint waypointToReach;

    public StateMovement(StateMachine stateMachine, IMovable objectToMove, Waypoint waypointToReach) : base(stateMachine)
    {
        //save references
        this.objectToMove = objectToMove;
        this.waypointToReach = waypointToReach;
    }

    public override void Enter()
    {
        base.Enter();

        //calculate speed
        Vector3 targetPosition = objectToMove.CalculateTargetPosition(waypointToReach);
        objectToMove.CalculateObjectSpeed(targetPosition);
    }

    public override void Execution()
    {
        base.Execution();

        //move object to waypoint
        objectToMove.MoveToWaypoint();
    }
}
