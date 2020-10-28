using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : StateMovement
{
    public EnemyMovement(StateMachine stateMachine, IMovable objectToMove, Waypoint waypointToReach) : base(stateMachine, objectToMove, waypointToReach)
    {
    }

    public override void Exit()
    {
        base.Exit();

        //if next waypoint has player, kill him
        if (waypointToReach.ObjectsOnWaypoint.Contains(GameManager.instance.player.gameObject))
        {
            GameManager.instance.player.PlayerGotKilled();
        }
        //else end turn
        else
        {
            Enemy enemy = stateMachine as Enemy;

            //if his a rock path, remove waypoint from the list
            CheckIsRockPath(enemy);

            GameManager.instance.LevelManager.EndEnemyTurn(enemy);
        }
    }

    void CheckIsRockPath(Enemy enemy)
    {
        //if this is a rock path, remove waypoint from the list
        if (enemy.PathToRock.Contains(waypointToReach))
            enemy.PathToRock.Remove(waypointToReach);
    }
}
