﻿
public class StateMovement : State
{
    IMovable objectToMove;
    protected Waypoint waypointToReach;

    public StateMovement(StateMachine stateMachine, IMovable objectToMove, Waypoint waypointToReach) : base(stateMachine)
    {
        //save references
        this.objectToMove = objectToMove;
        this.waypointToReach = waypointToReach;
    }

    public override void Enter()
    {
        base.Enter();

        //calculate necessary to move object
        objectToMove.SetTargetPosition(waypointToReach);

        //start movement
        stateMachine.StartCoroutine(objectToMove.Move(waypointToReach));
    }

    public override void Exit()
    {
        base.Exit();

        //set position of every object on this waypoint
        waypointToReach.SetPositionsOnWaypoint();
    }
}
