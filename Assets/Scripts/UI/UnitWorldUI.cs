using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    private TextMeshProUGUI actionPoints;
    private Image healthBar;
    private Unit unit;
    private HealthSystem healthSystem;

    private void Awake()
    {
        actionPoints = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        healthBar = transform.GetChild(1).transform.GetChild(1).GetComponent<Image>();
        unit = transform.parent.GetComponent<Unit>();
        healthSystem = transform.parent.GetComponent<HealthSystem>();

        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void Start()
    {
        UpdateActionPoints();
        UpdateHealthBar();
    }

    private void UpdateActionPoints()
    {
        actionPoints.text = unit.ActionPoints.ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = healthSystem.GetHealthNormalized();
    }
}
