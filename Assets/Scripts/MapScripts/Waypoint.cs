using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Waypoint : MonoBehaviour
{
    //Used this to know where the character can move
    [Header("Walkable Points")]
    [SerializeField] Waypoint[] walkableWaypoints = default;

    public Waypoint[] WalkableWaypoints => walkableWaypoints;

    [Header("Debug")]
    [SerializeField] int x = 0;
    [SerializeField] int y = 0;

    public int X => x;
    public int Y => y;

    public List<GameObject> ObjetsOnWaypoint { get; private set; }

    //set waypoint coordinates
    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
