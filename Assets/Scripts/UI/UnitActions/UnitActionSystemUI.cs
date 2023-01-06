using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPref;
    [SerializeField] private Transform actionButtonsContainer;
    [SerializeField] private TextMeshProUGUI actionPoints;

    private List<UnitActionButtonUI> actionButtons;

    private void Awake()
    {
        actionButtons = new List<UnitActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectUnitChanged += UnitActionSystem_OnSelectUnitChanged;
        UnitActionSystem.Instance.OnSelectActionChanged += UnitActionSystem_OnSelectActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void CreateUnitActionButtons()
    {
        foreach(Transform button in actionButtonsContainer)
        {
            Destroy(button.gameObject);
        }

        actionButtons.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;

        if (!selectedUnit)
            return;

        foreach(BaseAction baseAction in selectedUnit.BaseActions)
        {
            Transform actionButton = Instantiate(actionButtonPref, actionButtonsContainer);
            UnitActionButtonUI unitActionButtonUI =  actionButton.GetComponent<UnitActionButtonUI>();
            unitActionButtonUI.SetBaseAction(baseAction);

            actionButtons.Add(unitActionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach (UnitActionButtonUI actionButton in actionButtons)
        {
            actionButton.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;

        if (!selectedUnit)
            return;

        actionPoints.text = $"Action points: {selectedUnit.ActionPoints}";
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
}
