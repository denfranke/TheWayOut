using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform unitRagdollPref;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollInstance = Instantiate(unitRagdollPref, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollInstance.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}
