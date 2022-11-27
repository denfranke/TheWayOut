using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshProUGUI gCost;
    [SerializeField] private TextMeshProUGUI hCost;
    [SerializeField] private TextMeshProUGUI fCost;
    [SerializeField] private SpriteRenderer isWalkableIndicator;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);

        pathNode = (PathNode)gridObject;
    }

    protected override void Update()
    {
        base.Update();

        gCost.text = pathNode.GCost.ToString();
        hCost.text = pathNode.HCost.ToString();
        fCost.text = pathNode.FCost.ToString();
        isWalkableIndicator.color = pathNode.IsWalkable ? Color.green : Color.red;
    }
}
