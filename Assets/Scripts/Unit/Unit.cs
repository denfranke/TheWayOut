using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    [SerializeField] private bool isEnemy;
    [SerializeField] private int actionPoints;

    private int actionPointsMax;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    private HealthSystem healthSystem;

    private GridPosition gridPosition;

    private BaseAction[] baseActions;
    private List<Loot> lootItems;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActions = GetComponents<BaseAction>();
        lootItems = new List<Loot>();
    }

    private void Start()
    {
        actionPointsMax = actionPoints;

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if((isEnemy && !TurnSystem.Instance.IsPlayerTurn) || (!isEnemy && TurnSystem.Instance.IsPlayerTurn))
        {
            actionPoints = actionPointsMax;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
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

    public void Damage(int damageAmount)
    {
        healthSystem.TakeDamage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetWorldPosition() { return transform.position; }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in baseActions)
        {
            if (baseAction is T) return (T)baseAction;
        }

        return null;
    }

    public T GetLoot<T>() where T : Loot
    {
        foreach (Loot loot in lootItems)
        {
            if (loot is T)
            {
                return (T)loot;
            }
        }

        return null;
    }

    public void AddLoot(Loot loot) 
    {
        lootItems.Add(loot);
    }

    public GridPosition GridPosition { get { return gridPosition; } }
    public BaseAction[] BaseActions { get { return baseActions; } }
    public int ActionPoints { get { return actionPoints; } }
    public bool IsEnemy { get { return isEnemy; } }
}
