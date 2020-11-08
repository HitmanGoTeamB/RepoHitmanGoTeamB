using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Path
{
    Waypoint startWaypoint;
    Waypoint endWaypoint;

    public Path(Waypoint startWaypoint, Waypoint endWaypoint)
    {
        this.startWaypoint = startWaypoint;
        this.endWaypoint = endWaypoint;
    }
}

[AddComponentMenu("Hitman GO/Map/Show Path")]
public class ShowPath : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] LineRenderer line = default;
    [SerializeField] GameObject point = default;
    [SerializeField] GameObject endPoint = default;

    [Header("Important")]
    [SerializeField] float timeForTile = 0.5f;
    [SerializeField] float height = 2f;
    [SerializeField] float distFromPoint = 0.2f;
    [SerializeField] float distFromEndPoint = 0.3f;

    List<Waypoint> waypointsAlreadyEvaluated = new List<Waypoint>();
    List<Waypoint> waypointsToEvaluate = new List<Waypoint>();
    List<Path> pathsAlreadyEvaluated = new List<Path>();
    System.Action onEnd;

    public void CreatePath(System.Action onEnd)
    {
        //save function
        this.onEnd = onEnd;

        StartPath();
    }

    void StartPath()
    {
        Waypoint playerWaypoint = GameManager.instance.player.CurrentWaypoint;

        //create point on player waypoint
        Vector3 playerPosition = playerWaypoint.transform.position + Vector3.up * height;
        InstantiatePoint(playerWaypoint, playerPosition);

        //start create path from player
        waypointsAlreadyEvaluated.Add(playerWaypoint);
        PathsFrom(playerWaypoint);
    }

    void PathsFrom(Waypoint startWaypoint)
    {
        //foreach walkable waypoint, add to list and start coroutine
        foreach (Waypoint waypoint in startWaypoint.WalkableWaypoints)
        {
            //need to check path but also reverse
            Path path = new Path(startWaypoint, waypoint);
            Path reverse = new Path(waypoint, startWaypoint);

            //only if not already evaluated
            if(!pathsAlreadyEvaluated.Contains(path) && !pathsAlreadyEvaluated.Contains(reverse))
            {
                pathsAlreadyEvaluated.Add(path);
                waypointsToEvaluate.Add(waypoint);
                StartCoroutine(Path_Coroutine(startWaypoint, waypoint));
            }
        }

        //if there are no other waypoints to evaluate, call end function
        if(waypointsToEvaluate.Count <= 0)
        {
            onEnd?.Invoke();
        }
    }

    IEnumerator Path_Coroutine(Waypoint startWaypoint, Waypoint endWaypoint)
    {
        //create line and set parent
        LineRenderer lineRenderer = Instantiate(line, transform);

        //dist from center tile (used to not have line on point object)
        Vector3 direction = (endWaypoint.transform.position - startWaypoint.transform.position).normalized;
        Vector3 startDist = direction * (startWaypoint.IsFInalWaypoint ? distFromEndPoint : distFromPoint);
        Vector3 endDist = direction * (endWaypoint.IsFInalWaypoint ? distFromEndPoint : distFromPoint);

        //set start position
        Vector3 startPosition = startWaypoint.transform.position + Vector3.up * height + startDist;
        lineRenderer.SetPosition(0, startPosition);

        //set end position
        Vector3 endPosition = endWaypoint.transform.position + Vector3.up * height - endDist;

        //create line
        float delta = 0;
        while(delta < 1)
        {
            delta += Time.deltaTime / timeForTile;

            Vector3 deltaPosition = Vector3.Lerp(startPosition, endPosition, delta);
            lineRenderer.SetPosition(1, deltaPosition);

            yield return null;
        }

        //set final position (just to be sure)
        lineRenderer.SetPosition(1, endPosition);

        //if waypoint is not already evaluated, create point
        if (!waypointsAlreadyEvaluated.Contains(endWaypoint))
        {
            Vector3 pointPosition = endWaypoint.transform.position + Vector3.up * height;
            InstantiatePoint(endWaypoint, pointPosition);
        }

        //set waypoint evaluated
        waypointsToEvaluate.Remove(endWaypoint);
        waypointsAlreadyEvaluated.Add(endWaypoint);

        //and start new path
        PathsFrom(endWaypoint);
    }

    void InstantiatePoint(Waypoint waypoint, Vector3 position)
    {
        //get prefab if final waypoint or normal waypoint
        GameObject prefab = waypoint.IsFInalWaypoint ? endPoint : point;

        //if there is prefab, instantiate, set parent and position
        if(prefab)
        {
            GameObject pointObject = Instantiate(prefab, transform);
            pointObject.transform.position = position;
        }
    }
}
