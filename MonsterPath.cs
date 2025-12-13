using UnityEngine;

public class MonsterPath : MonoBehaviour
{
    public enum PathType
    {
        Loop,
        ReverseWhenCompleted
    }
    public Transform[] waypoints;
    public PathType pathType = PathType.Loop;


    private int direction = 1;
    int index;

    public Vector3 GetCurrentWayPoint()
    {
        return waypoints[index].position;
    }

    public Vector3 GetNextWaypoint()
    {
        if (waypoints.Length == 0) return transform.position;

        index = GetNextWaypointIndex();
        Vector3 nextWaypont = waypoints[index].position;

        return nextWaypont;
    }
    private int GetNextWaypointIndex()
    {
        //move to the next index
        index += direction;

        if (pathType == PathType.Loop)
        {
            index %= waypoints.Length;
        }
        else if (pathType == PathType.ReverseWhenCompleted)
        {

            if (index >= waypoints.Length || index < 0)
            {
                direction *= -1;
                index += direction * 2;
            }


        }
        return index;
    }

    //make path visible in unity editor

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        Gizmos.color = Color.white;

        //draw lines between waypoints
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        //Loop back to the start if the path type is loop
        if (pathType == PathType.Loop)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }

        Gizmos.color = Color.red;

        //draw spheres at waypoints
        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
        }
    }

}
