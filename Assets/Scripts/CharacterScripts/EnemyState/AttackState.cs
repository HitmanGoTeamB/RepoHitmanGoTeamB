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

        //animation
        Animator anim = stateMachine.GetComponentInChildren<Animator>();
        anim.SetTrigger("Attack");

        Enemy enemy = stateMachine as Enemy;
        enemy.SoundMovement(true);
    }

    public override void Exit()
    {
        //kill player (this state is called when the enemy move on the player waypoint)
        GameManager.instance.player.PlayerGotKilled();

        base.Exit();
    }
}
