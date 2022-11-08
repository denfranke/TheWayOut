using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPref;
    [SerializeField] private Transform actionButtonsContainer;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectUnitChanged += UnitActionSystem_OnSelectUnitChanged;
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons()
    {
        foreach(Transform button in actionButtonsContainer)
        {
            Destroy(button.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;

        if (!selectedUnit) return;

        foreach(BaseAction baseAction in selectedUnit.BaseActions)
        {
            Transform actionButton = Instantiate(actionButtonPref, actionButtonsContainer);
            UnitActionButtonUI unitActionButtonUI =  actionButton.GetComponent<UnitActionButtonUI>();
            unitActionButtonUI.SetBaseAction(baseAction);
        }
    }

    private void UnitActionSystem_OnSelectUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }
}
