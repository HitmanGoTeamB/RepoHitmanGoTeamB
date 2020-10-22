using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Enemy enemy = stateMachine as Enemy;
        enemy.CheckForPlayerInRange();
    }
}
