using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float fSmoothSpeed = 0.125f;

    private void Start()
    {
        transform.position = player.position + offset;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, fSmoothSpeed);
        transform.position = smoothedPosition;
    }
}
