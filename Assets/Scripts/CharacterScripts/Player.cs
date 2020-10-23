using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] float rockThrowTime = 1;

    public float RockThrowTime => rockThrowTime;

    void Awake()
    {
        SetState(new Wait(this));
    }

    void Update()
    {
        state?.Execution();
    }

    /// <summary>
    /// Called from Level Manager on start player turn
    /// </summary>
    public void ActivePlayer()
    {
        SetState(new PlayerWaitInput(this));
    }

    /// <summary>
    /// Called from enemy when kill player
    /// </summary>
    public void PlayerGotKilled()
    {
        //animazione di morte
        //chiama un endgame
        //REMEMBER this can be called more times, but it has to work only one time
        //TODO
        GameManager.instance.LevelManager.EndGame(false);
    }
}
