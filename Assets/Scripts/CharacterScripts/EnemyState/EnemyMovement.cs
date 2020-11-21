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

        //get references
        Animator anim = stateMachine.GetComponentInChildren<Animator>();

        if (IsOnPlayer())
        {
            anim.SetTrigger("Attack");
        }
        else
        {
            anim.SetTrigger("Move");
        }

        Enemy enemy = stateMachine as Enemy;
        enemy.SoundMovement(IsOnPlayer());
    }

    public override void Exit()
    {
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

        base.Exit();
    }

    void CheckIsRockPath(Enemy enemy)
    {
        //if this is a rock path, remove waypoint from the list
        if (enemy.PathToRock.Contains(waypointToReach))
        {
            enemy.PathToRock.Remove(waypointToReach);

            enemy.Rotate();
        }
    }

    bool IsOnPlayer()
    {
        //all player in the waypoint you are moving into
        return waypointToReach.GetObjectsOnWaypoint<Player>().Count > 0;
    }
}
