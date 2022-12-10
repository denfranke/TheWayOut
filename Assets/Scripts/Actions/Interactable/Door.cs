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
        DoorLock.OpenDoor += DoorLock_OpenDoor;
    }

    private void DoorLock_OpenDoor(object sender, EventArgs e)
    {
        animator.SetBool("OpenDoor", true);

        Pathfinding.Instance.SetIsWalkableGridPosition(new GridPosition(5, 7), true);
        Pathfinding.Instance.SetIsWalkableGridPosition(new GridPosition(4, 7), true);
    }
}
