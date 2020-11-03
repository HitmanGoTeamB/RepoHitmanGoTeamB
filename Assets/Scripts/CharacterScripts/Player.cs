using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LookCamera))]
[AddComponentMenu("Hitman GO/Characters/Player")]
public class Player : Character
{
    [Header("Time animation throw rock")]
    [SerializeField] float rockThrowTime = 1;

    public float RockThrowTime => rockThrowTime;

    private bool isAlive = true;

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
        if(isAlive == true)
        {
            GetComponentInChildren<Animator>().SetTrigger("Death");

            GameManager.instance.LevelManager.EndGame(false);
            isAlive = false;
        }
        
    }
}
