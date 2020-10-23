using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitInput : State
{
    public PlayerWaitInput(StateMachine stateMachine) : base(stateMachine)
    {
    }

    Dictionary<Vector2Int, Waypoint> waypointsAround = new Dictionary<Vector2Int, Waypoint>();

    public override void Enter()
    {
        Player player = stateMachine as Player;

        //fill waypoints around me
        player.GetAllWaypointsAroundMe(waypointsAround);
    }

    public override void Execution()
    {
        IMovable objectToMove = null;
        Waypoint waypointToMove = null;

        //wait input, then set waypoint
        if(Input.GetKeyDown(KeyCode.W))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            waypointToMove = objectToMove.GetWaypointToMove(waypointsAround[Vector2Int.up], false);
            
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            waypointToMove = objectToMove.GetWaypointToMove(waypointsAround[Vector2Int.down], false);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            waypointToMove = objectToMove.GetWaypointToMove(waypointsAround[Vector2Int.left], false);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            waypointToMove = objectToMove.GetWaypointToMove(waypointsAround[Vector2Int.right], false);
        }

        //if there is a waypoint, change state to movement
        if(waypointToMove != null)
        {
            stateMachine.SetState(new PlayerMovement(stateMachine, objectToMove, waypointToMove));
        }
    }
}
