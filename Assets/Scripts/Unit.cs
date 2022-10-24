using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;

    // Update is called once per frame
    void Update()
    {
        float stopDistance = .1f;

        if (Input.GetMouseButtonDown(0))
        {
            Move(MousePointer.GetPosition());
        }

        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            float moveSpeed = 4f;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
