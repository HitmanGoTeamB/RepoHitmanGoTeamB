using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : Character, IMovable
{
    //to use when set pathfinding
    public List<Waypoint> PathToRock { get; private set; } = new List<Waypoint>();

    void Awake()
    {
        SetState(new Wait(this));
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
