using UnityEngine;

[AddComponentMenu("Hitman GO/Rock")]
public class Rock : MonoBehaviour
{
    Waypoint currentWaypoint;

    void Start()
    {
        //get the waypoint where is the rock
        GetCurrentWaypoint();

        //add to objects on waypoint
        currentWaypoint.AddObjectToWaypoint(this.gameObject);
    }

    void GetCurrentWaypoint()
    {
        //find current waypoint with a raycast to the down
        RaycastHit hit;
        Physics.Raycast(this.transform.position + Vector3.up, Vector3.down, out hit);
        currentWaypoint = hit.transform.gameObject.GetComponent<Waypoint>();
    }
}
