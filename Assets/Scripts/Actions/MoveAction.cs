using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    private List<Vector3> positions;
    private int currentPositionIndex;

    private void Update()
    {
        if (!isActive) return;

        float stopDistance = .1f;

        Vector3 targetPosition = positions[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotateSpeed = 30f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if(currentPositionIndex >= positions.Count)
            {
                ActionComplete();
                OnStopMoving?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void TakeAction(GridPosition targetPosition, Action OnActionComplete)
    {
        List<GridPosition> path = Pathfinding.Instance.FindPath(unit.GridPosition, targetPosition, out int pathLength);

        currentPositionIndex = 0;

        positions = new List<Vector3>();

        foreach(GridPosition gridPosition in path)
            positions.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));

        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(OnActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GridPosition;

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                if (unitGridPosition == testGridPosition)
                    continue;

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                    continue;

                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                    continue;

                int pathDistanceMultiplayer = 10;
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) >
                    maxMoveDistance * pathDistanceMultiplayer)
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = 0;

        if (unit.TryGetComponent<ShootAction>(out ShootAction shootAction))
            targetCountAtGridPosition = shootAction.GetTargetCountAtPosition(gridPosition);
        else if(unit.TryGetComponent<SwordAction>(out SwordAction swordAction))
            targetCountAtGridPosition = swordAction.GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }

    public override string GetActionName() { return "Move"; }

    public override int GetActionPointsCost() { return 1; }
}