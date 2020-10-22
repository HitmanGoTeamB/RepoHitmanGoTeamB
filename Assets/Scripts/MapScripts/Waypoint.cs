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

    #endregion
}
