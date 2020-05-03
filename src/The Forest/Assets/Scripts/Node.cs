using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapElement<Node>
{
    public bool       traversable;
    public Vector2    worldPosition;
    public Vector2Int gridPosition;

    public int gCost;                                   //"Koszt" dojścia od początku drogi do tej kratki
    public int hCost;                                   //"Koszt" dojścia z tej kratki do końca
    public int fCost { get { return gCost + hCost; } }  //Zsumowane koszty
    public int HeapIndex { get; set; }                  //Numer w kopcu

    public Node parent;                                 //Poprzednia kratka w najoptymalniejszej drodze

    public Node(bool _traversable, Vector2 _worldPosition, Vector2Int _gridPosition)
    {
        traversable = _traversable;
        worldPosition = _worldPosition;
        gridPosition = _gridPosition;
    }
    public int CompareTo(Node node)
    {
        int compare = fCost.CompareTo(node.fCost);  //Porównaj zsumowane koszty
        if(compare == 0)
        {
            compare = hCost.CompareTo(node.hCost);  //Jeśli takie same, porównaj koszty heurystyczne
        }
        return -compare;
    }

}
