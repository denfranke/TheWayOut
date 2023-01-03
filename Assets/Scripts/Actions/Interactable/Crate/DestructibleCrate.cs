using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestructibleCrate : MonoBehaviour
{
    [SerializeField] private Transform crateDestroyedPref;

    public static event EventHandler OnAnyDestroyed;

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
        Transform crateDestroyedInstance = Instantiate(crateDestroyedPref, transform.position, transform.rotation);
        ApplyExplosionToChildren(crateDestroyedInstance, 50f, transform.position, 10f);
        LevelGrid.Instance.RemoveInteractableAtGridPosition(gridPosition, GetComponent<Crate>());

        Destroy(gameObject);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }

    public GridPosition GridPosition { get { return gridPosition; } }
}
