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
        Waypoint forwardWaypoint;

        //check player in range
        if (enemy.CheckForPlayerInRange(out forwardWaypoint))
        {
            //if in range, move to kill him
            stateMachine.SetState(new AttackState(stateMachine, stateMachine.GetComponent<IMovable>(), forwardWaypoint));
        }
        else
        {
            //else end turn
            stateMachine.SetState(new Wait(stateMachine));

            stateMachine.StartCoroutine(EndTurnInIdle());
        }
    }

    IEnumerator EndTurnInIdle()
    {
        LevelManager levelManager = GameManager.instance.LevelManager;

        //wait minimum time turn
        yield return new WaitForSeconds(levelManager.MinimumEnemyTurnDuration);

        //end turn
        levelManager.EndEnemyTurn(stateMachine as Enemy);
    }
}
