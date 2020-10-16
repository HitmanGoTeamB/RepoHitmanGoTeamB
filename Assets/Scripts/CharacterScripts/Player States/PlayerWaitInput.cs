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

        //wait input
        if(Input.GetKeyDown(KeyCode.W))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            //set waypoint
            waypointToMove = objectToMove.GetWaypointToMove(Vector3.forward);            
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            //set waypoint
            waypointToMove = objectToMove.GetWaypointToMove(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            //set waypoint
            waypointToMove = objectToMove.GetWaypointToMove(Vector3.forward);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            objectToMove = stateMachine.GetComponent<IMovable>();

            //set waypoint
            waypointToMove = objectToMove.GetWaypointToMove(Vector3.forward);
        }

        //if there is a waypoint, then change state to movement
        if(waypointToMove != null)
        {
            stateMachine.SetState(new StateMovement(stateMachine, objectToMove, waypointToMove));
        }
    }
}
