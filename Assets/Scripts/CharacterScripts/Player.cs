using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LookCamera))]
[AddComponentMenu("Hitman GO/Characters/Player")]
public class Player : Character
{
    [Header("Time animation throw rock")]
    [SerializeField] float rockThrowTime = 1;

    [Header("Models")]
    [SerializeField] GameObject normalModel = default;
    [SerializeField] GameObject throwRockModel = default;

    public float RockThrowTime => rockThrowTime;

    private bool isAlive = true;

    Animator anim;

    void Awake()
    {
        NormalPose();
        SetState(new Wait(this));

        //set animator reference
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        state?.Execution();

        Pause(Input.GetKeyDown(KeyCode.Escape));
    }

    void Pause(bool pauseInput)
    {
        if(pauseInput)
        {
            //resume
            if (GameManager.instance.uiManager.IsPauseOrEndGame())
            {
                GameManager.instance.uiManager.PauseMenu(false);
            }
            //pause game
            else
            {
                GameManager.instance.uiManager.PauseMenu(true);
            }
        }
    }

    /// <summary>
    /// Called from Level Manager on start player turn
    /// </summary>
    public void ActivePlayer()
    {
        //wait input to move
        if (normalModel.activeInHierarchy)
            SetState(new PlayerWaitInput(this));
        //else wait input to throw rock
        else
            SetState(new PlayerWaitThrowInput(this));
    }

    /// <summary>
    /// Called from enemy when kill player
    /// </summary>
    public void PlayerGotKilled()
    {
        //REMEMBER this can be called more times, but it has to work only one time
        if(isAlive == true)
        {
            isAlive = false;

            //animation
            GetComponent<LookCamera>().enabled = false;
            anim.SetTrigger("Death");

            //call end game
            GameManager.instance.LevelManager.EndGame(false);
        }
        
    }

    public void NormalPose()
    {
        normalModel.SetActive(true);
        throwRockModel.SetActive(false);
    }

    public void ThrowRockPose()
    {
        normalModel.SetActive(false);
        throwRockModel.SetActive(true);
    }
}
