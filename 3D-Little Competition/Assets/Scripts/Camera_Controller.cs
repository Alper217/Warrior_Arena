using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float rotationSpeed = 5f;
    public float minY = -35f, maxY = 60f;

    public float collisionOffset = 0.2f;
    public float minDistance = 1f;
    public LayerMask collisionLayers;

    private float currentYaw = 0f;
    private float currentPitch = 10f;

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 rawDesiredPosition = target.position + rotation * offset;

        // Kamera çarpýþma kontrolü
        Vector3 direction = (rawDesiredPosition - target.position).normalized;
        float maxDistance = offset.magnitude;
        Vector3 finalPosition = rawDesiredPosition;

        if (Physics.Raycast(target.position, direction, out RaycastHit hit, maxDistance, collisionLayers))
        {
            float adjustedDistance = Mathf.Max(hit.distance - collisionOffset, minDistance);
            finalPosition = target.position + direction * adjustedDistance;
        }

        transform.position = finalPosition;
        transform.LookAt(target);
    }

    public Quaternion GetCameraRotation()
    {
        return Quaternion.Euler(0f, currentYaw, 0f);
    }
}
