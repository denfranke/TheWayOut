using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    [SerializeField] private int damageAmount;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private enum State
    {
        Aiming,
        Shooting,
        CoolOff,
    }

    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private State state;
    private float stateTimer;
    private int maxShootDistance = 7;
    private Unit targetUnit;
    private bool canShootBullet;

    void Update()
    {
        if (!isActive) 
            return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;

            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;

            case State.CoolOff:
                break;
        }

        if (stateTimer <= 0)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        targetUnit.Damage(damageAmount);
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;

            case State.Shooting:
                state = State.CoolOff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;

            case State.CoolOff:
                ActionComplete();
                break;
        }
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        GridPosition unitGridPosition = unit.GridPosition;
        return GetValidActionGridPositions(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositions(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GridPosition;

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                    continue;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) 
                    continue;

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy == unit.IsEnemy)
                    continue;

                float unitShoulderHeight = 1.7f;
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight,
                    shootDirection,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    obstaclesLayerMask))
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        ActionStart(OnActionComplete);
    }

    public override string GetActionName() { return "Shoot"; }
    public override int GetActionPointsCost() { return 1; }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + (int)Math.Round( (1 - targetUnit.GetHealthNormalized()) * 100f ),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositions(gridPosition).Count;
    }

    public Unit TargetUnit { get { return targetUnit; } }
    public int MaxShootDistance { get { return maxShootDistance; } }
}
