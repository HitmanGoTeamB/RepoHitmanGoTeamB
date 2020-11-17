using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrelevelState : State
{
    LevelManager levelManager;
    Coroutine waitCinemachine;

    public PrelevelState(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        //show cinemachine, then show path, then start game
        //if restarted level, call show path immediatly, then show cinemachine and start game
        levelManager = stateMachine as LevelManager;

        if (GameManager.instance.showPath && levelManager.isAgainSameLevel)
            GameManager.instance.showPath.CreatePath(null, true);

        //start coroutine for cinemachine
        if (waitCinemachine != null)
            levelManager.StopCoroutine(waitCinemachine);

        waitCinemachine = levelManager.StartCoroutine(WaitCinemachine());
    }

    IEnumerator WaitCinemachine()
    {
        //wait cinemachine
        yield return new WaitForSeconds(levelManager.TimeCinemachine);

        //then do prelevel
        DoPrelevel();
    }

    void DoPrelevel()
    {
        //show path if component in scene
        if (GameManager.instance.showPath && levelManager.isAgainSameLevel == false)
            GameManager.instance.showPath.CreatePath(EndPrelevel, false);
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
