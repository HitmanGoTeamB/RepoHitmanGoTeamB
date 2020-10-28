using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : StateMovement
{
    public EnemyMovement(StateMachine stateMachine, IMovable objectToMove, Waypoint waypointToReach) : base(stateMachine, objectToMove, waypointToReach)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        if (waypointToReach.ObjectsOnWaypoint.Contains(GameManager.instance.player.gameObject))
        {
            GameManager.instance.player.PlayerGotKilled();
        }
        else
        {
            Enemy enemy = stateMachine as Enemy;

            enemy.PathToRock.RemoveAt(0);
            GameManager.instance.LevelManager.EndEnemyTurn(enemy);
        }
    }

}
