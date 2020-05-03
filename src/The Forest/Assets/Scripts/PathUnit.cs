using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUnit : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    Vector3[] path;
    int targetIdx;

    Vector3 currentWaypoint;
    bool followingPath = false;
    bool stopCommand = false;

    public void GoToTarget()
    {
        PathManager.RequestPath(transform.position, target.position, OnPathFound);
    }
    public void OnPathFound(Vector3[] newPath, bool foundPath)
    {
        if(foundPath && newPath.Length > 0)
        {
            path = newPath;
            targetIdx = 0;
            if(currentWaypoint != path[0]) { currentWaypoint = path[0]; }   //Podmiana obecnego waypointu, żeby zapewnić gładkość ruchu
            if (!followingPath) { StartCoroutine(FollowPath()); }
        }

    }
    public void Stop()
    {
        stopCommand = true;
    }
    public void Come()
    {
        stopCommand = false;
    }
    IEnumerator FollowPath()
    {
        followingPath = true;
        while(true)
        {
            while(stopCommand) { yield return null; }   //Zatrzymanie jednostki na komendę
            if(transform.position == currentWaypoint)
            {
                targetIdx++;
                if(targetIdx >= path.Length)
                {
                    followingPath = false;
                    yield break;
                }
                currentWaypoint = path[targetIdx];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }       //Korutyna do podążania za ścieżką

    //Debug
    private void OnDrawGizmos()
    {
        if(path != null)
        {
            Vector3 from = transform.position;
            Vector3 to;
            for (int i = 0; i < path.Length; i++)
            {
                to = path[i];
                Gizmos.color = Color.red;
                Gizmos.DrawLine(from, to);
                from = to;
            }
            if(path.Length <= 1)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, target.position);
            }
        }
    }
}
