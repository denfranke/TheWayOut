using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPref;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance" + transform + " " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        gridSystem = new GridSystem<PathNode>(width, height, cellSize,
           (GridSystem<PathNode> GridSystem, GridPosition gridPosition)
           => new PathNode(gridPosition));

        gridSystem.VisualizeGridDebugObjects(gridDebugObjectPref);

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffset = 5f;
                if(Physics.Raycast(worldPosition + Vector3.down * raycastOffset, Vector3.up,
                    raycastOffset * 2f,
                    obstaclesLayerMask))
                {
                    GetNode(x, z).IsWalkable = false;
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> open = new List<PathNode>();
        List<PathNode> closed = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        open.Add(startNode);

        for(int x = 0; x < gridSystem.Width; x++)
        {
            for (int z = 0; z < gridSystem.Height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.GCost = int.MaxValue;
                pathNode.HCost = 0;
                pathNode.CalculateFCost();
                pathNode.ParentPathNode = null;
            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalculateDistance(startGridPosition, endGridPosition);
        startNode.CalculateFCost();

        while(open.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(open);

            if(currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            open.Remove(currentNode);
            closed.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbours(currentNode))
            {
                if (closed.Contains(neighbourNode)) continue;
                if (!neighbourNode.IsWalkable)
                {
                    closed.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GCost + CalculateDistance(currentNode.GridPosition, neighbourNode.GridPosition);
                if(tentativeGCost < neighbourNode.GCost)
                {
                    neighbourNode.ParentPathNode = currentNode;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = CalculateDistance(neighbourNode.GridPosition, endGridPosition);
                    neighbourNode.CalculateFCost();

                    if(!open.Contains(neighbourNode))
                    {
                        open.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodes = new List<PathNode>();
        pathNodes.Add(endNode);

        PathNode currentNode = endNode;
        while(currentNode.ParentPathNode != null)
        {
            pathNodes.Add(currentNode.ParentPathNode);
            currentNode = currentNode.ParentPathNode;
        }

        pathNodes.Reverse();

        List<GridPosition> gridPositions = new List<GridPosition>();
        foreach(PathNode pathNode in pathNodes)
        {
            gridPositions.Add(pathNode.GridPosition);
        }

        return gridPositions;
    }

    public int CalculateDistance(GridPosition from, GridPosition to)
    {
        GridPosition distance = to - from;
        int xDistance = Mathf.Abs(distance.x);
        int zDistance = Mathf.Abs(distance.x);
        int remaining = Mathf.Abs(xDistance - zDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostPathNode = pathNodes[0];
        for(int i = 0; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].FCost < lowestFCostPathNode.FCost)
                lowestFCostPathNode = pathNodes[i];
        }

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighbours(PathNode currentNode)
    {
        List<PathNode> neighbours = new List<PathNode>();
        GridPosition gridPosition = currentNode.GridPosition;

        if(gridPosition.x - 1 >= 0)
        {
            neighbours.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

            if(gridPosition.z - 1 >= 0)
                neighbours.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));

            if (gridPosition.z + 1 < gridSystem.Height)
                neighbours.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
        }

        if (gridPosition.x + 1 < gridSystem.Width)
        {
            neighbours.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

            if (gridPosition.z - 1 >= 0)
                neighbours.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));

            if (gridPosition.z + 1 < gridSystem.Height)
                neighbours.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
        }

        if (gridPosition.z - 1 >= 0)
            neighbours.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));

        if (gridPosition.z + 1 < gridSystem.Height)
            neighbours.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));

        return neighbours;
    }
}
