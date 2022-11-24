using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode parentPathNode;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public void CalculateFCost() { fCost = GCost + HCost; }

    public PathNode ParentPathNode { get { return parentPathNode; } set { parentPathNode = value; } }

    public int GCost{ get { return gCost;} set { gCost = value; } }
    public int HCost { get { return hCost; } set { hCost = value; } }
    public int FCost { get { return fCost; } }

    public GridPosition GridPosition { get { return gridPosition; } }
}
