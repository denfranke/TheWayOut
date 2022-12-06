using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private Transform grenadeExplodeVFXPref;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    public static event EventHandler OnAnyGrenadeExploded;

    private Vector3 targetPosition;
    private Action onGrenadeActionComplete;
    private float totalDistance;
    private Vector3 positionXZ;

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;
        int moveSpeed = 15;

        positionXZ += moveDirection * moveSpeed * Time.deltaTime;

        float currentDistance = Vector3.Distance(positionXZ, targetPosition);
        float currentDistanceNormalized = 1 - currentDistance / totalDistance;

        float maxHeight = totalDistance / 2f;
        float positionY = arcYAnimationCurve.Evaluate(currentDistanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = 0.2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliders = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach(Collider collider in colliders)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(60);
                }

                if(collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();
                }
            }

            Instantiate(grenadeExplodeVFXPref, targetPosition + Vector3.up * 1f, Quaternion.identity);
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            onGrenadeActionComplete();
            trailRenderer.transform.parent = null;

            Destroy(gameObject);
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeActionComplete)
    { 
        this.onGrenadeActionComplete = onGrenadeActionComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
