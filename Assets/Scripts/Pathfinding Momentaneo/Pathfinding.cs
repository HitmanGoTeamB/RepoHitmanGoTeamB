using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding 
{
    //pathfinding algorithm
    public static List<Waypoint> FindPath(Waypoint StartWaypoint, Waypoint TargetWaypoint)
    {
        //open list is all waypoint that are not checked
        List<Waypoint> OpenList = new List<Waypoint>();
        //cloase list is all waypoint that are already checked
        List<Waypoint> ClosedList = new List<Waypoint>();
        //add the start node to openlist
        OpenList.Add(StartWaypoint);

        //loop
        while (OpenList.Count > 0)
        {
            //Current = node in OPEN with the lowest F cost
            Waypoint CurrentWaypoint = OpenList[0];
            for(int i = 1; i < OpenList.Count; i++)
            {
                if(OpenList[i].fCost < CurrentWaypoint.fCost || OpenList[i].fCost == CurrentWaypoint.fCost && OpenList[i].hCost < CurrentWaypoint.hCost)
                {
                    CurrentWaypoint = OpenList[i];
                }
            }

            //remove Current from openlist
            OpenList.Remove(CurrentWaypoint);
            //add Current to closedlist
            ClosedList.Add(CurrentWaypoint);

            //se sei alla fine 
            if (CurrentWaypoint == TargetWaypoint)
                return CreatePath(StartWaypoint, CurrentWaypoint);

            foreach (Waypoint Neighbour in CurrentWaypoint.WalkableWaypoints)
            {
                //see if neighbour is inside closed list and is already checked
                if (ClosedList.Contains(Neighbour))
                    continue;

                //calculate cost of Neighbour and check if is inside open list
                int newCostToNeighbour = CurrentWaypoint.gCost + GetDistance(CurrentWaypoint, Neighbour);
                if(newCostToNeighbour < Neighbour.gCost || !OpenList.Contains(Neighbour))
                {
                    Neighbour.gCost = newCostToNeighbour;
                    Neighbour.hCost = GetDistance(Neighbour, TargetWaypoint);
                    Neighbour.parentWaypoint = CurrentWaypoint;

                    if (!OpenList.Contains(Neighbour))
                        OpenList.Add(Neighbour);
                }
            }
        }

        return null;
    }

    //method that calculate cost(distance) between 2 points
    static int GetDistance(Waypoint WaypointA, Waypoint WaypointB)
    {
        int distanceX = Mathf.Abs(WaypointA.X - WaypointB.X);
        int distanceY = Mathf.Abs(WaypointA.Y - WaypointB.Y);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    //method that create the path you need to move through
    static List<Waypoint> CreatePath(Waypoint startWaypoint, Waypoint LastWaypoint)
    {
        List<Waypoint> path = new List<Waypoint>();
        Waypoint currentWaypoint = LastWaypoint;

        while(currentWaypoint != startWaypoint)
        {
            path.Add(currentWaypoint);
            currentWaypoint = currentWaypoint.parentWaypoint;
        }
        path.Reverse();

        return path;
    }

}
