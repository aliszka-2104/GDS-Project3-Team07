using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    PathManager pathManager;
    PathGrid pathGrid;

    private void Awake()
    {
        pathManager = GetComponent<PathManager>();
        pathGrid = GetComponent<PathGrid>();
    }
    public void StartPathing(Vector3 start, Vector3 target)
    {
        StartCoroutine(FindPath(start, target));
    }
    IEnumerator FindPath(Vector3 start, Vector3 target)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathFound = false;

        Node startNode  = pathGrid.GetNodeFromWorldPoint(start);
        Node targetNode = pathGrid.GetNodeFromWorldPoint(target);

        if (startNode.traversable && targetNode.traversable)            //Eliminacja oczywistego przypadku
        {
            Heap<Node> openSet = new Heap<Node>(pathGrid.AreaSize);     //Kopiec na otwarte kratki
            HashSet<Node> closedSet = new HashSet<Node>();              //Hashset na zamknięte
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node current = openSet.RemoveFirst();
                closedSet.Add(current);

                if (current == targetNode)                              //Return condition
                {
                    pathFound = true;
                    break;
                }

                foreach (Node neighbor in pathGrid.GetNeighbors(current))   //Ewaluacja sąsiadów
                {
                    if (!neighbor.traversable || closedSet.Contains(neighbor)) { continue; }
                    int newCostToNeighbor = current.gCost + GetNodeDistance(current, neighbor);
                    if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCostToNeighbor;
                        neighbor.hCost = GetNodeDistance(neighbor, targetNode);
                        neighbor.parent = current;

                        if (!openSet.Contains(neighbor)) { openSet.Add(neighbor); }
                    }
                }
            }
        }
        yield return null;
        if (pathFound)
        {
            waypoints = GetPath(startNode, targetNode);             //Wyciągnięcie ścieżki
        }
        pathManager.FinishedProcessingPath(waypoints, pathFound);   //Callback do managera
    }   //Korutyna, żeby uniknąć freezowania za każdym wywołaniem
    Vector3[] GetPath(Node start, Node end)
    {
        List<Node> path = new List<Node>();     //Backtracking
        Node current = end;
        while(current != start)
        {
            path.Add(current);
            current = current.parent;
        }
        Vector3[] waypoints = SimplePath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }
    Vector3[] SimplePath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();      //Usunięcie niepotrzebnych waypointów
        Vector2 dirOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 dirNew = new Vector2(path[i - 1].gridPosition.x - path[i].gridPosition.x, path[i - 1].gridPosition.y - path[i].gridPosition.y);
            if(dirNew != dirOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            dirOld = dirNew;
        }
        return waypoints.ToArray();
    }
    int GetNodeDistance(Node a, Node b)
    {
        Vector2Int distanceOnAxis = new Vector2Int(0, 0);
        distanceOnAxis.x = Mathf.Abs(a.gridPosition.x - b.gridPosition.x);
        distanceOnAxis.y = Mathf.Abs(a.gridPosition.y - b.gridPosition.y);
        int distance = (distanceOnAxis.x > distanceOnAxis.y) ? (141 * distanceOnAxis.y + 100 * (distanceOnAxis.x - distanceOnAxis.y)) : (141 * distanceOnAxis.x + 100 * (distanceOnAxis.y - distanceOnAxis.x));
        return distance;
    }
}
