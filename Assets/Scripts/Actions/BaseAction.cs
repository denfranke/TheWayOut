using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action OnActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action OnCompleteAction);

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void  ActionStart(Action OnActionComplete)
    {
        isActive = true;
        this.OnActionComplete = OnActionComplete;
    }

    protected void ActionComplete()
    {
        isActive = false;
        OnActionComplete();
    }
}
