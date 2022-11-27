using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridVisualPref;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }

    public static GridSystemVisual Instance { get; private set; }

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = 
                    Instantiate(gridVisualPref, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitActionSystem.Instance.OnSelectActionChanged += UnitActionSystem_OnSelectActionChanged;
        UnitActionSystem.Instance.OnSelectUnitChanged += UnitActionSystem_OnSelectUnitChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;


        UpdateGridVisuals();
    }

    private void UnitActionSystem_OnSelectUnitChanged(object sender, EventArgs e)
    {
        UpdateGridVisuals();
    }

    private void UnitActionSystem_OnSelectActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisuals();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisuals();
    }

    private void UpdateGridVisuals()
    {
        HideAllGridVisuals();

        Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;
        BaseAction selectedAction = UnitActionSystem.Instance.SelectedAction;

        GridVisualType gridVisualType;

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;

            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;

            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridVisualsRange(selectedUnit.GridPosition, shootAction.MaxShootDistance, GridVisualType.RedSoft);
                break;

            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;

            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridVisualsRangeSquare(selectedUnit.GridPosition, swordAction.MaxSwordDistance, GridVisualType.RedSoft);
                break;

            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }

        ShowGridVisuals(selectedAction.GetValidActionGridPositions(), gridVisualType);
    }

    private void HideAllGridVisuals()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    private void ShowGridVisuals(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach(GridPosition gridPosition in gridPositions)
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].
                Show(GetGridVisualTypeMaterial(gridVisualType));
    }

    private void ShowGridVisualsRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositions = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range) continue;

                gridPositions.Add(testGridPosition);
            }
        }

        ShowGridVisuals(gridPositions, gridVisualType);
    }

    private void ShowGridVisualsRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositions = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                gridPositions.Add(testGridPosition);
            }
        }

        ShowGridVisuals(gridPositions, gridVisualType);
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach(GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterials)
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType)
                return gridVisualTypeMaterial.material;

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
        return null;
    }
}
