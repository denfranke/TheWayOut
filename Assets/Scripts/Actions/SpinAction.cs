using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    void Update()
    {
        if (!isActive) return;

        float spinAmount = 360f * Time.deltaTime;
        totalSpinAmount += spinAmount;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);

        if (totalSpinAmount >= 360) ActionComplete();
    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        totalSpinAmount = 0;
        ActionStart(OnActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GridPosition;

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override string GetActionName() { return "Spin"; }

    public override int GetActionPointsCost() { return 2; }
}
