using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Start()
    {
        SetState(new PlayerWaitInput(this));
    }

    void Update()
    {
        state?.Execution();
    }
}
