using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        cinemachineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZooming();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection.z = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection.z = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection.x = 1;
        }

        float moveSpeed = 15.0f;
        Vector3 moveVector = transform.forward * moveDirection.z + transform.right * moveDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleZooming()
    {
        float zoomAmount = 1f;

        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
        }

        float zoomSpeed = 5f;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
