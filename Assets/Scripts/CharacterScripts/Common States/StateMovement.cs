
public class StateMovement : State
{
    IMovable objectToMove;
    protected Waypoint waypointToReach;
    Waypoint startWaypoint;

    public StateMovement(StateMachine stateMachine, IMovable objectToMove, Waypoint waypointToReach) : base(stateMachine)
    {
        //save references
        this.objectToMove = objectToMove;
        this.waypointToReach = waypointToReach;
    }

    public override void Enter()
    {
        base.Enter();

        Character character = stateMachine as Character;
        startWaypoint = character.CurrentWaypoint;

        //calculate necessary to move object
        objectToMove.SetTargetPosition(waypointToReach);

        //start movement
        stateMachine.StartCoroutine(objectToMove.Move(waypointToReach));
    }

    public override void Exit()
    {
        base.Exit();

        //set position of every object on previous waypoint
        startWaypoint.SetPositionsOnWaypoint();

        //set position of every object on this waypoint
        waypointToReach.SetPositionsOnWaypoint();
    }
}
