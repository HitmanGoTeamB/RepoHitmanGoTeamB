using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitInput : State
{
    public PlayerWaitInput(StateMachine stateMachine) : base(stateMachine)
    {
    }

    Dictionary<Vector2Int, Waypoint> waypointsAround = new Dictionary<Vector2Int, Waypoint>();

    Camera cam;
    bool isMoving;
    Vector2 startInputPosition;

    Animator anim;

    public override void Enter()
    {
        Player player = stateMachine as Player;

        //fill waypoints around me
        player.GetAllWaypointsAroundMe(waypointsAround);

        //get references
        cam = Camera.main;
        anim = stateMachine.GetComponentInChildren<Animator>();
    }

    public override void Execution()
    {
        //check on click and on release (touch or mouse)
#if UNITY_ANDROID
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.GetTouch(0);

        if(!isMoving && touch.phase == TouchPhase.Began)
        {
            OnClick();
        }
        else if(isMoving && (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended))
        {
            OnRelease();
        }
#else
        if (!isMoving && Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnClick();
        }
        else if(isMoving && Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnRelease();
        }
#endif
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
        int layer = CreateLayer.LayerOnly("Player");

        //if hit player, save start input position and start moving
        if (Physics.Raycast(ray, 100, layer))
        {
            startInputPosition = inputPosition;
            isMoving = true;

            anim.SetTrigger("OnClick");
        }
    }

    void OnRelease()
    {
        //stop moving
        isMoving = false;

        Vector2 inputPosition = GetInput();
        Ray ray = cam.ScreenPointToRay(inputPosition);
        int layer = CreateLayer.LayerOnly("Player");

        //be sure doesn't hit player again (no movement)
        if (Physics.Raycast(ray, 100, layer))
        {
            anim.SetTrigger("OnRelease");
            return;
        }

        Vector2 movement = inputPosition - startInputPosition;

        //move on y axis
        if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
        {
            if (movement.y > 0)
                Move(new Vector2Int(0, 1));     //move up
            else
                Move(new Vector2Int(0, -1));    //move down
        }
        //move on x axis
        else
        {
            if (movement.x > 0)
                Move(new Vector2Int(1, 0));     //move right
            else
                Move(new Vector2Int(-1, 0));    //move left
        }
    }

#endregion

    void Move(Vector2Int direction)
    {
        IMovable objectToMove = stateMachine.GetComponent<IMovable>();
        Waypoint waypointToMove = objectToMove.GetWaypointToMove(waypointsAround[direction], false);

        //if there is a waypoint, change state to movement
        if (waypointToMove != null)
        {
            stateMachine.SetState(new PlayerMovement(stateMachine, objectToMove, waypointToMove));
        }
        else
        {
            anim.SetTrigger("OnRelease");
        }
    }
}
