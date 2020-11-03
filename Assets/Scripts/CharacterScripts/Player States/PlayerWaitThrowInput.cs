using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitThrowInput : State
{
    public PlayerWaitThrowInput(StateMachine stateMachine) : base(stateMachine)
    {
    }

    Dictionary<Vector2Int, Waypoint> waypointsAround = new Dictionary<Vector2Int, Waypoint>();
    Player player;

    Camera cam;
    bool isThrowing;
    Waypoint startInputPosition;

    Animator anim;

    public override void Enter()
    {
        player = stateMachine as Player;

        //fill waypoints around me
        player.GetAllWaypointsAroundMe(waypointsAround);

        //get references
        cam = Camera.main;
        anim = stateMachine.GetComponentInChildren<Animator>();
    }

    public override void Execution()
    {
        //wait input, then set waypoint
        //TODO 
        //do animation

        //check on click and on release (touch or mouse)
#if UNITY_ANDROID
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.GetTouch(0);

        if(!isThrowing && touch.phase == TouchPhase.Began)
        {
            OnClick();
        }
        else if(isThrowing && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended))
        {
            OnRelease();
        }
#else
        if (!isThrowing && Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnClick();
        }
        else if (isThrowing && Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnRelease();
        }
#endif
    }

    public override void Exit()
    {
        //coroutine to throw
        stateMachine.StartCoroutine(WaitAnimationToEnd());
    }

    #region private API

    Vector2 GetInput()
    {
        //return touch position or mouse position
#if UNITY_ANDROID
        return Input.GetTouch(0).position;
#else
        return Input.mousePosition;
#endif
    }

    void OnClick()
    {
        Vector2 inputPosition = GetInput();
        Ray ray = cam.ScreenPointToRay(inputPosition);
        int layer = CreateLayer.LayerAllExcept("Player");   //layer all except player, unique colliders in scene are player and waypoints

        RaycastHit hit;

        //if hit waypoint, save start input waypoint and start throwing
        if (Physics.Raycast(ray, out hit, 100, layer))
        {
            startInputPosition = hit.transform.GetComponentInParent<Waypoint>();
            isThrowing = true;
        }
    }

    void OnRelease()
    {
        //stop throwing
        isThrowing = false;

        Vector2 inputPosition = GetInput();
        Ray ray = cam.ScreenPointToRay(inputPosition);
        int layer = CreateLayer.LayerAllExcept("Player");   //layer all except player, unique colliders in scene are player and waypoints

        RaycastHit hit;

        //if hit waypoint, check is the same waypoint and throw rock
        if (Physics.Raycast(ray, out hit, 100, layer))
        {
            if(hit.transform.GetComponentInParent<Waypoint>() == startInputPosition)
            {
                ThrowRock(startInputPosition);
            }
        }
    }

    void ThrowRock(Waypoint waypoint)
    {
        //if there is a waypoint, throw
        if (waypoint != null)
        {
            //set enemies path finding and set player state to Wait
            GameManager.instance.LevelManager.SetEnemiesPathFinding(waypoint);
            stateMachine.SetState(new Wait(stateMachine));

            anim.SetTrigger("Throw Rock");

            //TEMP
            foreach(Renderer renderer in waypoint.GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.cyan;
            }
        }
    }

    #endregion

    IEnumerator WaitAnimationToEnd()
    {
        //wait
        yield return new WaitForSeconds(player.RockThrowTime);

        //set player wait input state
        player.NormalPose();
        stateMachine.SetState(new PlayerWaitInput(stateMachine));
    }
}
