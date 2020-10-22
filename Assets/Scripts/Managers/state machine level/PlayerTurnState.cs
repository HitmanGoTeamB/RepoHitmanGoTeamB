using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : State
{

    public PlayerTurnState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        GameManager.instance.LevelManager.StartPlayerTurn();       
    }

    
}
    
