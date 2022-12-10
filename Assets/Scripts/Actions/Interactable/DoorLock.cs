using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour, IInteractable
{
    [SerializeField] Material greenMaterial;

    private MeshRenderer meshRenderer;
    private GridPosition gridPosition;
    public Action OpenDoor;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
    }

    public void Interact(Unit interactUnit, Action OnInteractionComplete)
    {
        OnInteractionComplete();
        meshRenderer.material = greenMaterial;
        OpenDoor?.Invoke();
        LevelGrid.Instance.RemoveInteractableAtGridPosition(gridPosition, this);
    }
}
