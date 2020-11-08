using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Hitman GO/Managers/UI Manager")]
public class UIManager : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float height = 2;
    [SerializeField] float tileSize = 4;

    [Header("Rock")]
    [SerializeField] Canvas rockPoint = default;
    [SerializeField] Canvas rockArea = default;
    [SerializeField] float rockAreaAnimation = 1;

    Pooling<Canvas> rockPoints = new Pooling<Canvas>();
    Pooling<Canvas> rockAreas = new Pooling<Canvas>();

    #region private API

    Vector3 WaypointPosition(Waypoint waypoint)
    {
        return waypoint.transform.position + Vector3.up * height;
    }

    IEnumerator RockAreaAnimation(Transform area)
    {
        //set start and end size
        Vector3 startSize = area.localScale;
        float areaEffect = GameManager.instance.LevelManager.RockAreaEffect;
        float size = areaEffect + areaEffect * 2;
        Vector3 endSize = new Vector3(size, size, size);

        //animation
        float delta = 0;
        while(delta < 1)
        {
            delta += Time.deltaTime / rockAreaAnimation;
            area.localScale = Vector3.Lerp(startSize, endSize, delta);

            yield return null;
        }

        //deactive area on end, and reset size
        area.gameObject.SetActive(false);
        area.localScale = startSize;
    }

    #endregion

    #region public API

    public void ShowRockPoints(Dictionary<Vector2Int, Waypoint> waypoints)
    {
        //foreach waypoint
        foreach(Waypoint waypoint in waypoints.Values)
        {
            if (waypoint != null)
            {
                //pooling instantiate rock point
                Canvas point = rockPoints.Instantiate(rockPoint, transform);
                point.transform.position = WaypointPosition(waypoint);
            }
        }
    }

    public void HideRockPoints()
    {
        rockPoints.DeactiveAll();
    }

    public void ShowRockArea(Waypoint waypoint)
    {
        //pooling instantiate rock area
        Canvas area = rockAreas.Instantiate(rockArea, transform);
        area.transform.position = WaypointPosition(waypoint);

        StartCoroutine(RockAreaAnimation(area.transform));
    }

    #endregion
}
