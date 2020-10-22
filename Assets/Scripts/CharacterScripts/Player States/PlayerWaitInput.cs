using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitInput : State
{
    public PlayerWaitInput(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Execution()
    {
        IMovable objectToMove = null;
        Waypoint waypointToMove = null;

        //wait input, then set waypoint
        if(Input.GetKeyDown(KeyCode.W))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            waypointToMove = objectToMove.GetWaypointToMove(Vector2Int.up);
            
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            waypointToMove = objectToMove.GetWaypointToMove(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            waypointToMove = objectToMove.GetWaypointToMove(Vector2Int.left);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            waypointToMove = objectToMove.GetWaypointToMove(Vector2Int.right);
        }

        //if there is a waypoint, change state to movement
        if(waypointToMove != null)
        {
            stateMachine.SetState(new StateMovement(stateMachine, objectToMove, waypointToMove));
        }
    }
}
