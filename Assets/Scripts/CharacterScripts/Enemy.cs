using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : Character, IMovable
{
    private List<Waypoint> pathToRock = new List<Waypoint>();
    public List<Waypoint> PathToRock => pathToRock;

    void Awake()
    {
        SetState(new Wait(this));
    }

    /// <summary>
    /// Called from Level Manager on start enemy turn
    /// </summary>
    public void ActiveEnemy()
    {
        if (pathToRock.Count > 0)
            SetState(new EnemyMovement(this, GetComponent<IMovable>(), pathToRock[0]));
        else
            SetState(new IdleState(this));
    }

    /// <summary>
    /// Called from idle state. If there is enemy in range, kill him. Else end turn
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
        pathToRock = Pathfinding.FindPath(CurrentWaypoint, waypointToReach);
    }

    public void Die()
    {
        GameManager.instance.LevelManager.enemiesInScene.Remove(this);
        Destroy(this.gameObject);

        //TODO
        //muovilo alla nello spazio adiacente alla scacchiera
    }
}
