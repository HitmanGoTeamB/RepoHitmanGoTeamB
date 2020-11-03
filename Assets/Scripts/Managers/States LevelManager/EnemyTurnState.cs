using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : State
{
    public EnemyTurnState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //call start enemy turn
        GameManager.instance.LevelManager.StartEnemyTurn();
    }
}
