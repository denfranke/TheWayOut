using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Crate : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform lootPref;

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
    }

    public void Interact(Unit interactUnit, Action OnInteractionComplete)
    {
        OnInteractionComplete();
        GetComponent<DestructibleCrate>().Damage();

        Transform lootInstance = Instantiate(lootPref, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        lootInstance.TryGetComponent<Key>(out Key key);
        key.SetLootAtGridPosition(this.gameObject);
    }
}
