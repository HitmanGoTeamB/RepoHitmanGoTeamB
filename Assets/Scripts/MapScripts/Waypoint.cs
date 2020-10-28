using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Waypoint : MonoBehaviour
{
    #region variables

    //Used this to know where the character can move
    [Header("Walkable Points")]
    [SerializeField] Waypoint[] walkableWaypoints = default;

    public Waypoint[] WalkableWaypoints => walkableWaypoints;

    [Header("Is Final Waypoint?")]
    [SerializeField] bool isFinalWaypoint = false;

    public bool IsFInalWaypoint => isFinalWaypoint;

    [Header("Debug")]
    [SerializeField] int x = 0;
    [SerializeField] int y = 0;

    public int X => x;
    public int Y => y;

    //values for pathfinding
    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost => gCost + hCost;

    public Waypoint parentWaypoint;

    //objects on this waypoint (player, rock, enemies, ecc...)
    public List<GameObject> ObjectsOnWaypoint;// { get; private set; }

    #endregion

    #region public API

    //set waypoint coordinates
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
}
