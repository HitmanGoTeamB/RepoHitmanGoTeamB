using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitThrowInput : State
{
    public PlayerWaitThrowInput(StateMachine stateMachine) : base(stateMachine)
    {
    }

    Dictionary<Vector2Int, Waypoint> waypointsAround = new Dictionary<Vector2Int, Waypoint>();
    Player player;

    public override void Enter()
    {
        player = stateMachine as Player;

        //fill waypoints around me
        player.GetAllWaypointsAroundMe(waypointsAround);
    }

    public override void Execution()
    {
        //wait input, then set waypoint
        //TODO 
        //do animation
        if (Input.GetKeyDown(KeyCode.W))
        {
            ThrowRock(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ThrowRock(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ThrowRock(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ThrowRock(Vector2Int.right);
        }
    }

    void ThrowRock(Vector2Int direction)
    {
        //if there is a waypoint, throw
        if (waypointsAround[direction] != null)
        {
            GameManager.instance.LevelManager.SetEnemiesPathFinding(waypointsAround[direction]);
            waypointsAround[direction].gameObject.GetComponentInChildren<Renderer>().material.color = Color.cyan;
            stateMachine.SetState(new Wait(stateMachine));
        }
    }

    public override void Exit()
    {
        //coroutine to throw
        stateMachine.StartCoroutine(WaitAnimationToEnd());
    }

    IEnumerator WaitAnimationToEnd()
    {
        //wait
        yield return new WaitForSeconds(player.RockThrowTime);

        //end player turn
        GameManager.instance.LevelManager.EndPlayerTurn();
    }
}
