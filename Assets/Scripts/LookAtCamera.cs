using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Vector3 directionToCamera = (mainCamera.transform.position - transform.position).normalized;
        transform.LookAt(transform.position + directionToCamera * -1f);
    }
}
