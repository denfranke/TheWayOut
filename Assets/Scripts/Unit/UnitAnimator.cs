using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform bulletProjectilePref;
    [SerializeField] Transform sword;
    [SerializeField] Transform rifle;
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform handLeft;
    [SerializeField] Transform handRight;

    private Vector3 rightHandRiflePosition = new Vector3(8, 8, -2.6f);
    private Vector3 rightHandRifleRotation = new Vector3(4.15f, 52.3f, -128.2f);

    private Vector3 leftHandRiflePosition = new Vector3(12f, -3f, 13f);
    private Vector3 leftHandRifleRotation = new Vector3(-6f, 226f, -382f);

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

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
        }

        if (TryGetComponent<GrenadeAction>(out GrenadeAction grenadeAction))
        {
            grenadeAction.OnGrenadeActionStarted += GrenadeAction_OnGrenadeActionStarted;
            grenadeAction.OnGrenadeActionCompleted += GrenadeAction_OnGrenadeActionCompleted;
        }
    }

    private void Start()
    {
        if (GetComponent<ShootAction>())
            animator.SetBool("IsShooter", true);

        if (GetComponent<SwordAction>())
            animator.SetBool("IsWarrior", true);
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void GrenadeAction_OnGrenadeActionStarted(object sender, EventArgs e)
    {
        rifle.transform.parent = handLeft;
        rifle.transform.localPosition = leftHandRiflePosition;
        rifle.transform.localRotation = Quaternion.Euler(leftHandRifleRotation);

        animator.SetTrigger("GrenadeThrow");
    }

    private void GrenadeAction_OnGrenadeActionCompleted(object sender, EventArgs e)
    {
        rifle.transform.parent = handRight;
        rifle.transform.localPosition = rightHandRiflePosition;
        rifle.transform.localRotation = Quaternion.Euler(rightHandRifleRotation);

        animator.SetTrigger("GrenadeThrow");
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e) 
    {
        animator.SetBool("IsWalking", true); 
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileInstance = Instantiate(bulletProjectilePref, shootPoint.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileInstance.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = shootPoint.transform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void EquipSword()
    {
        sword.transform.gameObject.SetActive(true);
        rifle.transform.gameObject.SetActive(false);
    }

    private void EquipRifle()
    {
        sword.transform.gameObject.SetActive(false);
        rifle.transform.gameObject.SetActive(true);
    }
}
