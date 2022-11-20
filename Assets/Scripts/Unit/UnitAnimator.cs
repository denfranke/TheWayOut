using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform bulletProjectilePref;
    [SerializeField] Transform shootPoint;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e) { animator.SetBool("IsWalking", true); }

    private void MoveAction_OnStopMoving(object sender, EventArgs e) { animator.SetBool("IsWalking", false); }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileInstance = Instantiate(bulletProjectilePref, shootPoint.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileInstance.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = shootPoint.transform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }
}
