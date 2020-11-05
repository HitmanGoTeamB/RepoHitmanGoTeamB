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

        //TODO cinemachine

        //show path if component in scene
        if (GameManager.instance.showPath)
            GameManager.instance.showPath.CreatePath(EndPrelevel);
        //else end prelevel
        else
            EndPrelevel();
    }

    void EndPrelevel()
    {
        //just start player turn
        stateMachine.SetState(new PlayerTurnState(stateMachine));
    }
}
