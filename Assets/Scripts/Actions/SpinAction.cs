using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{
    private float totalSpeenAmount;

    void Update()
    {
        if (!isActive) return;

        float spinAmount = 360f * Time.deltaTime;
        totalSpeenAmount += spinAmount;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);

        if (totalSpeenAmount >= 360)
        {
            isActive = false;
            OnActionComplete();
        }
    }
    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        this.OnActionComplete = OnActionComplete;
        isActive = true;
        totalSpeenAmount = 0;
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GridPosition;

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
}
