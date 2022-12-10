using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        FindObjectOfType<DoorLock>().OpenDoor += DoorLock_OpenDoor;
    }

    private void DoorLock_OpenDoor()
    {
        animator.SetBool("OpenDoor", true);
    }

    public void SetWalkablePass()
    {
        Pathfinding.Instance.UpdateWalkablePass();
    }
}
