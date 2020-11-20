using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Hitman GO/Managers/UI Manager")]
public class UIManager : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float height = 2;

    [Header("Rock")]
    [SerializeField] Canvas rockPoint = default;
    [SerializeField] Canvas rockArea = default;
    [SerializeField] float rockAreaAnimation = 1;

    Pooling<Canvas> rockPoints = new Pooling<Canvas>();
    Pooling<Canvas> rockAreas = new Pooling<Canvas>();

    [Header("Canvas")]
    [SerializeField] GameObject[] panelInGame = default;
    [SerializeField] GameObject[] pauseMenu = default;
    [SerializeField] GameObject[] endMenu = default;
    [SerializeField] GameObject[] hintMenu = default;

    void Start()
    {
        PanelInGame(true);
        PauseMenu(false);
        EndMenu(false);
        HintMenu(false);
    }

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

    void ActiveElements(bool active, GameObject[] gameObjects)
    {
        foreach (GameObject go in gameObjects)
            go.SetActive(active);
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

    #region canvas

    public void PanelInGame(bool active)
    {
        ActiveElements(active, panelInGame);
    }

    public void PauseMenu(bool active)
    {
        ActiveElements(active, pauseMenu);
    }

    public void EndMenu(bool active)
    {
        ActiveElements(active, endMenu);

        //if active, be sure to deactive pause menu
        if (active)
            PauseMenu(false);
    }

    public void HintMenu(bool active)
    {
        ActiveElements(active, hintMenu);
    }

    #endregion

    #endregion
}
