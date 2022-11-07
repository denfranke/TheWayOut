using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private LayerMask unitLayerMask;

    private Unit selectedUnit;

    public event EventHandler OnSelectUnitChanged;

    public static UnitActionSystem Instance { get; private set; }

    private bool isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance" + transform + " " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (isBusy) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!TryHandleUnitSelection() && selectedUnit)
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MousePointer.GetPosition());
                if(selectedUnit.MoveAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    selectedUnit.MoveAction.Move(mouseGridPosition, ClearBusy);
                    SetBusy();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedUnit.SpinAction.Spin(ClearBusy);
            SetBusy();
        }
    }

    private void SetBusy() { isBusy = true; }
    private void ClearBusy() { isBusy = false; }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayerMask))
        {
            if (hit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit SelectedUnit { get{ return selectedUnit; }}
}
