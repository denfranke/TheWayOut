using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

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

        if (unit.IsEnemy) enemyUnits.Remove(unit);
        else friendlyUnits.Remove(unit);
    }

    public List<Unit> Units { get { return units; } }
    public List<Unit> FriendlyUnits { get { return friendlyUnits; } }
    public List<Unit> EnemyUnits { get { return enemyUnits; } }
}