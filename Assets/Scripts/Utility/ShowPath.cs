using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Hitman GO/Map/Show Path")]
public class ShowPath : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] LineRenderer line = default;
    [SerializeField] GameObject point = default;

    [Header("Important")]
    [SerializeField] float timeForTile = 1;
    [SerializeField] float height = 4.1f;

    List<Waypoint> waypointsAlreadyEvaluated = new List<Waypoint>();
    List<Waypoint> waypointsToEvaluate = new List<Waypoint>();
    System.Action onEnd;

    GameObject parent;
    GameObject Parent { 
        get 
        {
            if (parent == null)
            {
                parent = new GameObject("Path");
                parent.transform.position = Vector3.zero;
            }

            return parent;
        } }

    public void CreatePath(System.Action onEnd)
    {
        //save function
        this.onEnd = onEnd;

        //start create path from player
        waypointsAlreadyEvaluated.Add(GameManager.instance.player.CurrentWaypoint);
        PathsFrom(GameManager.instance.player.CurrentWaypoint);
    }

    void PathsFrom(Waypoint startWaypoint)
    {
        //foreach walkable waypoint, add to list and start coroutine
        foreach (Waypoint waypoint in startWaypoint.WalkableWaypoints)
        {
            //only if not already evaluated
            if (!waypointsAlreadyEvaluated.Contains(waypoint))
            {
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
        LineRenderer lineRenderer = Instantiate(line, Parent.transform);

        //set start position
        Vector3 startPosition = startWaypoint.transform.position + Vector3.up * height;
        lineRenderer.SetPosition(0, startPosition);

        //set end position
        Vector3 endPosition = endWaypoint.transform.position + Vector3.up * height;

        //create line
        float delta = 0;
        while(delta < 1)
        {
            delta += Time.deltaTime / timeForTile;

            Vector3 deltaPosition = Vector3.Lerp(startPosition, endPosition, delta);
            lineRenderer.SetPosition(1, deltaPosition);

            yield return null;
        }

        //set final position
        lineRenderer.SetPosition(1, endPosition);

        //create point, set parent and position
        if (point)
        {
            GameObject pointObject = Instantiate(point, transform);
            pointObject.transform.position = endPosition;
        }

        //set waypoint evaluated
        waypointsToEvaluate.Remove(endWaypoint);
        waypointsAlreadyEvaluated.Add(endWaypoint);

        //and start new path
        PathsFrom(endWaypoint);
    }
}
