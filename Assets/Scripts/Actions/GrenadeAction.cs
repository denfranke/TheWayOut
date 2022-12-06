using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Transform GrenadeProjectilePref;

    private enum State
    {
        Aiming,
        ThrowingStart,
        ThrowingEnd,
    }

    public event EventHandler OnGrenadeActionStarted;
    public event EventHandler OnGrenadeActionCompleted;

    private State state;
    private float stateTimer;

    private int maxThrowDistance = 7;
    private GridPosition targetPosition;
    private bool canThrowGrenade;

    private void Update()
    {
        if (!isActive)
            return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection = (LevelGrid.Instance.GetWorldPosition(targetPosition) - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;

            case State.ThrowingStart:
                break;

            case State.ThrowingEnd:
                if (canThrowGrenade)
                {
                    Throw();
                    canThrowGrenade = false;
                    OnGrenadeActionCompleted?.Invoke(this, EventArgs.Empty);
                    ActionComplete();
                }
                break;
        }

        if (stateTimer <= 0)
        {
            NextState();
        }
    }

    private void Throw()
    {
        Transform grenadeProjectileInstance = Instantiate(GrenadeProjectilePref, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileInstance.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(targetPosition, ActionComplete);
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.ThrowingStart;
                float aimingStateTime = 0.1f;
                stateTimer = aimingStateTime;
                break;

            case State.ThrowingStart:
                state = State.ThrowingEnd;
                float throwingStartStateTime = 2f;
                stateTimer = throwingStartStateTime;

                OnGrenadeActionStarted?.Invoke(this, EventArgs.Empty);

                break;

            case State.ThrowingEnd:
                stateTimer = 10f;
                canThrowGrenade = true;
                break;
        }
    }


    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 2,
        };
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GridPosition;

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance)
                    continue;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        targetPosition = gridPosition;

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        ActionStart(OnActionComplete);
    }
}
