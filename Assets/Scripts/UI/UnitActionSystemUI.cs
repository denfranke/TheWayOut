using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPref;
    [SerializeField] private Transform actionButtonsContainer;

    private List<UnitActionButtonUI> actionButtons;

    private void Awake()
    {
        actionButtons = new List<UnitActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectUnitChanged += UnitActionSystem_OnSelectUnitChanged;
        UnitActionSystem.Instance.OnSelectActionChanged += UnitActionSystem_OnSelectActionChanged;

        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void CreateUnitActionButtons()
    {
        foreach(Transform button in actionButtonsContainer)
        {
            Destroy(button.gameObject);
        }

        actionButtons.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;

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
}
