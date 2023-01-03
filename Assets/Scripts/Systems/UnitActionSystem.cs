using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private LayerMask unitLayerMask;

    public static UnitActionSystem Instance { get; private set; }

    private Unit selectedUnit;
    private BaseAction selectedAction;
    private bool isBusy;
    private string defaultSelectedUnitName = "UnitRifle";

    public event EventHandler OnSelectUnitChanged;
    public event EventHandler OnSelectActionChanged;
    public event EventHandler<bool> OnBusyActionChanged;
    public event EventHandler OnActionStarted;

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

    private void Start()
    {
        Unit unit = GameObject.Find(defaultSelectedUnitName).GetComponent<Unit>();
        SetSelectedUnit(unit);
    }

    private void Update()
    {
        if (isBusy) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (TryHandleUnitSelection()) return;
        if (!TurnSystem.Instance.IsPlayerTurn) return;

        HandleSelectedAction();
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayerMask))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit) return false;
                    if (unit.IsEnemy == true) return false;

                    SetSelectedUnit(unit);

                    return true;
                }
            }
        }

        return false;
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MousePointer.GetPosition());

            if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                if(selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
                {
                    SetBusy();
                    selectedAction.TakeAction(mouseGridPosition, ClearBusy);
                    OnActionStarted?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    private void SetBusy() 
    { 
        isBusy = true;
        OnBusyActionChanged?.Invoke(this, isBusy);
    }
    private void ClearBusy() 
    { 
        isBusy = false;
        OnBusyActionChanged?.Invoke(this, isBusy);
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        selectedAction = unit.GetAction<MoveAction>();
        OnSelectUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit SelectedUnit { get{ return selectedUnit; }}
    public BaseAction SelectedAction { get { return selectedAction; } set 
    { 
        selectedAction = value; 
        OnSelectActionChanged?.Invoke(this, EventArgs.Empty); 
    } }
}
