using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Key : Loot, IInteractable
{
    private GridPosition gridPosition;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 2, 0));
    }

    public void SetLootAtGridPosition(GameObject parent)
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(parent.transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        transform.Rotate(new Vector3(0, 0, 45));
    }

    public void Interact(Unit interactUnit, Action OnInteractionComplete)
    {
        OnInteractionComplete();
        interactUnit.AddLoot(this);
        LevelGrid.Instance.RemoveInteractableAtGridPosition(gridPosition, this);
        gameObject.SetActive(false);
    }
}