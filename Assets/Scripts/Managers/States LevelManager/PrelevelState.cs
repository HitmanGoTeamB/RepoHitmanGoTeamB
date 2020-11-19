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
        levelManager = stateMachine as LevelManager;

        //if restarted level, call show path immediatly, then show cinemachine and start game
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
        float timer = Time.time + levelManager.TimeCinemachine;
        while (Time.time < timer)
        {
            //if click, skip cinemachine
            if (CheckOnClick())
                break;

            yield return null;
        }

        //then do prelevel
        DoPrelevel();
    }

    bool CheckOnClick()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount <= 0)
            return false;

        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            return true;
        }

        return false;
#else
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            return true;
        }

        return false;
#endif
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
