using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private int hiddenUnits = 2;

    public static UnitManager Instance { get; private set; }

    public event EventHandler OnAllEnemiesDead;
    public event EventHandler OnAllAlliesDead;

    private List<Unit> units;
    private List<Unit> friendlyUnits;
    private List<Unit> enemyUnits;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance" + transform + " " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        units = new List<Unit>();
        friendlyUnits = new List<Unit>();
        enemyUnits = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        units.Add(unit);

        if (unit.IsEnemy) enemyUnits.Add(unit);
        else friendlyUnits.Add(unit);
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        units.Remove(unit);

        if (unit.IsEnemy)
        {
            enemyUnits.Remove(unit);
            if (enemyUnits.Count == 0 - hiddenUnits)
                OnAllEnemiesDead?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            friendlyUnits.Remove(unit);
            if (friendlyUnits.Count == 0)
                OnAllAlliesDead?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<Unit> Units { get { return units; } }
    public List<Unit> FriendlyUnits { get { return friendlyUnits; } }
    public List<Unit> EnemyUnits { get { return enemyUnits; } }

    public int HiddenUnits { get { return hiddenUnits; } set { hiddenUnits = value; } }
}