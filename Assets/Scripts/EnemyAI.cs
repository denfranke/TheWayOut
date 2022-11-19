using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForTurn,
        TakingTurn,
        Busy
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn) return;

        switch (state)
        {
            case State.WaitingForTurn:
                break;

            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (TryTakeAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;

            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if(!TurnSystem.Instance.IsPlayerTurn)
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    private bool TryTakeAction(Action OnActionComplete)
    {
        foreach(Unit enemyUnit in UnitManager.Instance.EnemyUnits)
        {
            if(TryTakeAction(enemyUnit, OnActionComplete))
                return true;
        }

        return false;
    }

    private bool TryTakeAction(Unit enemyUnit, Action OnActionComplete)
    {
        SpinAction spinAction = enemyUnit.SpinAction;

        GridPosition actionGridPosition = enemyUnit.GridPosition;

        if (spinAction.IsValidActionGridPosition(actionGridPosition))
        {
            if (enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
            {
                spinAction.TakeAction(actionGridPosition, OnActionComplete);
                return true;
            }
        }

        return false;
    }
}
