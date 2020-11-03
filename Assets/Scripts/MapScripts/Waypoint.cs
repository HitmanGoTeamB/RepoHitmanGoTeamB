using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Hitman GO/Map/Waypoint")]
[SelectionBase]
public class Waypoint : MonoBehaviour
{
    #region variables

    [Header("Walkable Points")]
    [Tooltip("Used this to know where the character can move")]
    [SerializeField] Waypoint[] walkableWaypoints = default;

    [Header("Is Final Waypoint?")]
    [SerializeField] bool isFinalWaypoint = false;

    [Header("Debug")]
    [SerializeField] int x = 0;
    [SerializeField] int y = 0;

    #region properties

    public Waypoint[] WalkableWaypoints => walkableWaypoints;

    public bool IsFInalWaypoint => isFinalWaypoint;

    public int X => x;
    public int Y => y;

    #endregion

    #region values for pathfinding

    public int gCost { get; set; }      //distance from start point
    public int hCost { get; set; }      //distance from end point
    public int fCost => gCost + hCost;  //sum of G cost and H cost

    //used to retrace path
    public Waypoint parentWaypoint { get; set; }

    #endregion

    //objects on this waypoint (player, rock, enemies, ecc...)
    public List<GameObject> ObjectsOnWaypoint { get; private set; } = new List<GameObject>();

    #endregion

    #region public API

    /// <summary>
    /// Set waypoint coordinates
    /// </summary>
    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void AddObjectToWaypoint(GameObject objectToAdd)
    {
        ObjectsOnWaypoint.Add(objectToAdd);
    }

    public void RemoveObjectFromWaypoint(GameObject objectToRemove)
    {
        ObjectsOnWaypoint.Remove(objectToRemove);
    }

    /// <summary>
    /// Get objects on waypoint with this class
    /// </summary>
    /// <typeparam name="T">class to look for</typeparam>
    /// <returns></returns>
    public List<T> GetObjectsOnWaypoint<T>() where T : Component
    {
        List<T> objectsOfThisType = new List<T>();

        //foreach object in the list
        foreach(GameObject obj in ObjectsOnWaypoint)
        {
            //if the object is of this type, add to the list
            T objInTheList = obj.GetComponent<T>();
            if (objInTheList)
            {
                objectsOfThisType.Add(objInTheList);
            }
        }

        //return the list
        return objectsOfThisType;
    }

    #endregion

    #region private API

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        //draw line to every walkable waypoint
        foreach (Waypoint waypoint in WalkableWaypoints)
            Gizmos.DrawLine(transform.position, waypoint.transform.position);
    }

    #endregion
}
