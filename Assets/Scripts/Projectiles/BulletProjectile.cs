using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] Transform bulletHitVfxPref;

    private TrailRenderer trailRenderer;
    private Vector3 targetPosition;

    private void Awake()
    {
        trailRenderer = transform.GetChild(0).GetComponent<TrailRenderer>();
    }

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float distanceBeforeShooting = Vector3.Distance(transform.position, targetPosition);

        float moveSpeed = 200f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float distanceAfterShooting = Vector3.Distance(transform.position, targetPosition);

        if(distanceBeforeShooting < distanceAfterShooting)
        {
            transform.position = targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitVfxPref, targetPosition, Quaternion.identity);
        }
    }
}
