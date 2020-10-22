using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrelevelState : State
{
    public PrelevelState(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.SetState(new PlayerTurnState(stateMachine));
    }
}
