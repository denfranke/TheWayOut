using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    [SerializeField] private bool isEnemy;

    private const int ACTION_POINTS_MAX = 5;

    public static event EventHandler OnAnyActionPointsChanged;

    private GridPosition currentGridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActions;
    private int actionPoints = ACTION_POINTS_MAX;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActions = GetComponents<BaseAction>();
    }

    private void Start()
    {
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(currentGridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != currentGridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, currentGridPosition, newGridPosition);
            currentGridPosition = newGridPosition;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if((isEnemy && !TurnSystem.Instance.IsPlayerTurn) || (!isEnemy && TurnSystem.Instance.IsPlayerTurn))
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointsCost()) return true;
        else return false;
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
            return false;
    }

    public void Damage()
    {
        Debug.Log("Damaged");
    }

    public Vector3 GetWorldPosition() { return transform.position; }

    public MoveAction MoveAction { get { return moveAction; } }
    public SpinAction SpinAction { get { return spinAction; } }
    public GridPosition GridPosition { get { return currentGridPosition; } }
    public BaseAction[] BaseActions { get { return baseActions; } }
    public int ActionPoints { get { return actionPoints; } }

    public bool IsEnemy { get { return isEnemy; } }
}
