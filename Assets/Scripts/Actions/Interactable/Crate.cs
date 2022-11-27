using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Crate : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform crateDestroyedPref;

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
    }

    public void Interact(Action OnInteractionComplete)
    {
        OnInteractionComplete();
        GetComponent<DestructibleCrate>().Damage();
    }
}
