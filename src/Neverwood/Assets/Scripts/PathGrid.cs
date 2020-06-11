using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid : MonoBehaviour
{
    public Vector2   worldSize;
    public float     nodeSize;
    public LayerMask traverseMask;                 //Maska warstwy obiektów terenu (nie do pokonania)
    public bool      debugGizmos;
    Node[,]          pathGrid;

    float            nodeDiameter;
    Vector2Int       gridSize;

    private void Awake()
    {
        gridSize = new Vector2Int(0, 0);
        nodeDiameter = nodeSize * 2;
        gridSize.x = Mathf.RoundToInt(worldSize.x / nodeDiameter);
        gridSize.y = Mathf.RoundToInt(worldSize.y / nodeDiameter);
        CreatePathGrid();
    }
    public Node GetNodeFromWorldPoint(Vector3 position)
    {
        Vector2 percent = new Vector2(0, 0);
        percent.x = Mathf.Clamp01((position.x + worldSize.x / 2) / worldSize.x);
        percent.y = Mathf.Clamp01((position.z + worldSize.y / 2) / worldSize.y);

        Vector2Int nodePos = new Vector2Int(0, 0);
        nodePos.x = Mathf.RoundToInt((gridSize.x - 1) * percent.x);
        nodePos.y = Mathf.RoundToInt((gridSize.y - 1) * percent.y);

        return pathGrid[nodePos.x, nodePos.y];
    }
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0) { continue; }
                Vector2Int neighborPos = new Vector2Int(0, 0);
                neighborPos.x = node.gridPosition.x + x;
                neighborPos.y = node.gridPosition.y + y;

                bool[] inside = new bool[2];
                inside[0] = neighborPos.x >= 0 && neighborPos.x < gridSize.x;
                inside[1] = neighborPos.y >= 0 && neighborPos.y < gridSize.y;
                
                if(inside[0] && inside[1])
                {
                    neighbors.Add(pathGrid[neighborPos.x, neighborPos.y]);
                }
            }
        }

        return neighbors;
    }   //Sąsiedzi kratki, potrzebne do A*
    public int AreaSize
    {
        get
        {
            return gridSize.x * gridSize.y;
        }
    }                         //Ile kratek w używanym obszarze
    void CreatePathGrid()                          //Populacja kratki
    {
        pathGrid = new Node[gridSize.x, gridSize.y];
        Vector2 bottomLeft = new Vector2(transform.position.x, transform.position.y) - Vector2.right * worldSize.x / 2 - Vector2.up * worldSize.y / 2;
        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                Vector2 worldPoint = bottomLeft + Vector2.right * (x * nodeDiameter + nodeSize) + Vector2.up * (y * nodeDiameter + nodeSize);
                bool traversable = Physics.OverlapSphere(new Vector3(worldPoint.x, 0, worldPoint.y), nodeSize, traverseMask).Length == 0;
                pathGrid[x, y] = new Node(traversable, new Vector3(worldPoint.x, 0, worldPoint.y), new Vector2Int(x, y));   //Ważne, ustalanie czy dane miejsce jest możliwe do pokonania
            }
        }
    }

    //Debug
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, 0, worldSize.y));
        if (pathGrid != null && debugGizmos)
        {
            foreach (Node node in pathGrid)
            {
                Gizmos.color = (node.traversable) ? new Color(1, 1, 1, 0.5f) : new Color(0, 0, 0, 0.5f);
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeSize * 2 - .04f));
            }
        }
    }
}
