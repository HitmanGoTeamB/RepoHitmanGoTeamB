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
        //start player turn
        GameManager.instance.LevelManager.StartPlayerTurn();       
    }    
}
    
