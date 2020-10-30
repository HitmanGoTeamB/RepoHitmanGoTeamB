﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : Character, IMovable
{
    [Header("Time animation rotation")]
    [SerializeField] float timeToRotate = 1;

    //to use when set pathfinding
    public List<Waypoint> PathToRock { get; private set; } = new List<Waypoint>();

    Coroutine rotate_Coroutine;

    void Awake()
    {
        SetState(new Wait(this));
    }

    IEnumerator Rotate_Coroutine(Quaternion lookRotation)
    {
        //set start variables
        Quaternion startRotation = transform.rotation;
        float delta = 0;

        //animation
        while(delta < 1)
        {
            delta += Time.deltaTime / timeToRotate;
            transform.rotation = Quaternion.Lerp(startRotation, lookRotation, delta);

            yield return null;
        }

        //set final to be sure
        transform.rotation = lookRotation;
    }

    public void Rotate()
    {
        //if there is a next waypoint in the path
        if(PathToRock != null && PathToRock.Count > 0)
        {
            //get rotation to reach
            Vector3 lookDirection = PathToRock[0].transform.position - CurrentWaypoint.transform.position;
            Quaternion lookRotation = Quaternion.FromToRotation(transform.forward, lookDirection) * transform.rotation;

            //be sure is going only one coroutine
            if (rotate_Coroutine != null)
                StopCoroutine(rotate_Coroutine);

            //start rotation coroutine (animation)
            rotate_Coroutine = StartCoroutine(Rotate_Coroutine(lookRotation));
        }
    }

    /// <summary>
    /// Called from Level Manager on start enemy turn
    /// </summary>
    public void ActiveEnemy()
    {
        //if there is a path, follow it
        if (PathToRock != null && PathToRock.Count > 0)
            SetState(new EnemyMovement(this, GetComponent<IMovable>(), PathToRock[0]));
        //else go in idle state
        else
            SetState(new IdleState(this));
    }

    /// <summary>
    /// Called from idle state. Check if there is player in front of enemy
    /// </summary>
    public bool CheckForPlayerInRange(out Waypoint forwardWaypoint)
    {
        //get transform forward in Vector2Int
        Vector2Int forwardtransform = new Vector2Int(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.z));

        //get waypoint
        forwardWaypoint = GameManager.instance.map.GetWaypointInDirection(CurrentWaypoint, forwardtransform);
        if (forwardWaypoint != null)
        {
            //if walkable and there is player on it
            if (CurrentWaypoint.WalkableWaypoints.Contains(forwardWaypoint))
            {
                if (forwardWaypoint.ObjectsOnWaypoint.Contains(GameManager.instance.player.gameObject))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void SetPathFinding(Waypoint waypointToReach)
    {
        //set path to rock
        PathToRock = Pathfinding.FindPath(CurrentWaypoint, waypointToReach);
        Rotate();
    }

    public void Die()
    {
        //remove from level manager list
        GameManager.instance.LevelManager.enemiesInScene.Remove(this);

        //TODO
        //muovilo alla nello spazio adiacente alla scacchiera
        Destroy(this.gameObject);
    }
}
