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

    void Awake()
    {
        NormalPose();
        SetState(new Wait(this));
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
        //animazione di morte
        //chiama un endgame
        //REMEMBER this can be called more times, but it has to work only one time
        //TODO
        if(isAlive == true)
        {
            GetComponent<LookCamera>().enabled = false;
            GetComponentInChildren<Animator>().SetTrigger("Death");

            GameManager.instance.LevelManager.EndGame(false);
            isAlive = false;
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
