using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        UnitActionSystem.Instance.OnSelectUnitChanged += UnitActionSystem_OnSelectUnitChanged;
        meshRenderer.enabled = false;
    }

    private void UnitActionSystem_OnSelectUnitChanged(object sender, EventArgs empty)
    {
        if (this.transform.parent.GetComponent<Unit>() == UnitActionSystem.Instance.SelectedUnit)
            meshRenderer.enabled = true;
        else
            meshRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectUnitChanged -= UnitActionSystem_OnSelectUnitChanged;
    }
}
