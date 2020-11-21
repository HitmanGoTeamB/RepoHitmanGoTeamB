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

    [Header("Sounds")]
    [SerializeField] AudioClip[] movementSound = default;
    [SerializeField] AudioClip[] attackSound = default;
    [SerializeField] AudioClip[] pickRockSound = default;
    [SerializeField] AudioClip[] throwRockSound = default;
    [SerializeField] AudioClip[] rockHitSound = default;

    public float RockThrowTime => rockThrowTime;

    private bool isAlive = true;

    Animator anim;
    AudioSource audio;

    void Awake()
    {
        NormalPose();
        SetState(new Wait(this));

        //set animator reference
        anim = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
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

        //play sound when rock hit ground
        audio.clip = rockHitSound[Random.Range(0, rockHitSound.Length)];
        audio.Play();
    }

    public void ThrowRockPose()
    {
        normalModel.SetActive(false);
        throwRockModel.SetActive(true);

        //play sound
        audio.clip = pickRockSound[Random.Range(0, pickRockSound.Length)];
        audio.Play();
    }

    public void SoundMovement(bool attack)
    {
        if(attack)
        {
            audio.clip = movementSound[Random.Range(0, movementSound.Length)];
        }
        else
        {
            audio.clip = attackSound[Random.Range(0, attackSound.Length)];
        }

        audio.Play();
    }

    public void ThrowRockSound()
    {
        audio.clip = throwRockSound[Random.Range(0, throwRockSound.Length)];
        audio.Play();
    }
}
