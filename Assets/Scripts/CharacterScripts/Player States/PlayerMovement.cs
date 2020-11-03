using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : StateMovement
{
    public PlayerMovement(StateMachine stateMachine, IMovable objectToMove, Waypoint waypointToReach) : base(stateMachine, objectToMove, waypointToReach)
    {
    }

    Animator anim;

    public override void Enter()
    {
        base.Enter();

        //get references
        anim = stateMachine.GetComponentInChildren<Animator>();

        if(IsOnEnemy())
        {
            anim.SetTrigger("Attack");
        }
        else
        {
            anim.SetTrigger("Move");
        }
    }

    public override void Exit()
    {
        //check if end game
        if (waypointToReach.IsFInalWaypoint)
        {
            GameManager.instance.LevelManager.EndGame(true);
        }

        //else check if there are rocks to throw
        else if(IsOnRock())
        {
            stateMachine.SetState(new PlayerWaitThrowInput(stateMachine));
        }
        //else check if there are enemies to kill, then kill 'em and end turn
        else if(IsOnEnemy())
        {
            KillEnemies();
            GameManager.instance.LevelManager.EndPlayerTurn();
        }
        //or end turn
        else
        {
            GameManager.instance.LevelManager.EndPlayerTurn();
        }
    }

    bool IsOnRock()
    {
        List<Rock> rocks = waypointToReach.GetObjectsOnWaypoint<Rock>();

        //remove every rock from the waypoint list and from the scene
        foreach (Rock rock in rocks)
        {
            waypointToReach.RemoveObjectFromWaypoint(rock.gameObject);
            Object.Destroy(rock.gameObject);
        }

        return rocks.Count > 0;
    }

    bool IsOnEnemy()
    {
        //all enemy in the waypoint you are moving into
        return waypointToReach.GetObjectsOnWaypoint<Enemy>().Count > 0;
    }

    void KillEnemies()
    {
        //all enemy in the waypoint you are moving into
        List<Enemy> enemies = waypointToReach.GetObjectsOnWaypoint<Enemy>();

        //remove every enemy from the waypoint list and kill 'em
        foreach (Enemy enemy in enemies)
        {
            waypointToReach.RemoveObjectFromWaypoint(enemy.gameObject);
            enemy.Die();
        }
    }
}
