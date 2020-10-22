using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMovement
{
    public AttackState(StateMachine stateMachine, IMovable objectToMove, Waypoint waypointToReach) : base(stateMachine, objectToMove, waypointToReach)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        //metodo player ucciso
        //GameManager.instance.player.
    }
}
