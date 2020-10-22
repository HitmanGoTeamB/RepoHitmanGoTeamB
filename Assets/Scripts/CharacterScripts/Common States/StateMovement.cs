
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

        //calculate necessary to move object
        objectToMove.SetTargetPosition(waypointToReach);

        //start movement
        stateMachine.StartCoroutine(objectToMove.Move(waypointToReach));
    }

    public override void Exit()
    {
        base.Exit();

        //when the player move
        if (stateMachine is Player)
        {
            Player player = stateMachine as Player;

            //check if end game
            if (player.CheckIsFinalWaypoint())
            {
                GameManager.instance.LevelManager.EndGame(true);
            }
            //or end turn
            else
            {
                GameManager.instance.LevelManager.EndPlayerTurn();
            }
        }
    }
}
