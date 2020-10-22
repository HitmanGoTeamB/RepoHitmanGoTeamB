using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Start()
    {
        SetState(new Wait(this));
    }

    void Update()
    {
        state?.Execution();
    }

    public void ActivePlayer()
    {
        SetState(new PlayerWaitInput(this));
    }

    public void PlayerGotKilled()
    {
        //animazione di morte
        //chiama un endgame
    }
}
