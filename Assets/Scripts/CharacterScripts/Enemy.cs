using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : Character, IMovable
{
    // Start is called before the first frame update
    void Start()
    {
        SetState(new Wait(this));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveEnemy()
    {
        SetState(new IdleState(this));
    }

    public void CheckForPlayerInRange()
    {
        //transform vector3 transform to vector2int
        //usa funzione nella mappa per waypoint
        //check se il waypoint è camminabile
        //check se il player e' sul quel waypoint
        //se il player e' li cambia stato in attacco
        //se il player non e' li vai in wait e comunica che hai finito il turno 

        Vector2Int forwardtransform = new Vector2Int(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.z));
        Waypoint forwardWaypoint = GameManager.instance.map.GetWaypointInDirection(CurrentWaypoint, forwardtransform);
        if (forwardWaypoint != null)
        {
            if (forwardWaypoint.WalkableWaypoints.Contains(forwardWaypoint) && forwardWaypoint.ObjetsOnWaypoint.Contains(GameManager.instance.player.gameObject))
            {
                SetState(new AttackState(this, this, forwardWaypoint));
                return;
            }
        }

        SetState(new Wait(this));
        GameManager.instance.LevelManager.EndEnemyTurn(this);
    }
}
