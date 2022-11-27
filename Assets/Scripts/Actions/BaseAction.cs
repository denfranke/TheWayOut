using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected Unit unit;
    protected bool isActive;
    protected Action OnActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action OnCompleteAction);

    public abstract List<GridPosition> GetValidActionGridPositions();

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositions();
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
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        OnActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActions = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositions();

        foreach(GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActions.Add(enemyAIAction);
        }

        if (enemyAIActions.Count > 0)
        {
            enemyAIActions.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActions[0];
        }
        else 
            return null;
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

    public Unit Unit { get { return unit; } }
}
