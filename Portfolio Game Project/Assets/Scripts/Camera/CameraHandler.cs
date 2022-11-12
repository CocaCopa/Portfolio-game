using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
    [Tooltip("Target to Follow")]
    [SerializeField] private Transform playerObj;

    [Space(10)]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform mainCamera;

    private float mouseX;
    private float mouseY;
    private float lookAngle = 0;
    private float pivotAngle = 0;

    [Header("---- Camera Stats ----")]
    [Range(0.03f, 0.2f)] [Tooltip("Camera will stop Smoothly")]
    [SerializeField] private float cameraSpeed = 0.2f;
    [Range(1000f, 6000f)] [Tooltip("Change the Horizontal mouse sensitivity")]
    [SerializeField] private float lookSensitivity = 0.18f;
    [Range(1000f, 6000f)] [Tooltip("Change the Vertical mouse sensitivity")]
    [SerializeField] private float pivotSensitivity = 0.15f;
    [SerializeField] private bool invertMouseY;

    private void Start() {

        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update() {

        Vector3 playerPos = playerObj.position + new Vector3(0, 1, 0);
        transform.position = playerPos;

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }

    private void LateUpdate() {

        mouseY = (invertMouseY) ? -mouseY : mouseY;
        HandleRotation(mouseX, mouseY, Time.deltaTime);
    }

    private void HandleRotation(float mouseX, float mouseY, float deltaTime) {

        lookAngle += (mouseX * lookSensitivity) * deltaTime;
        pivotAngle -= (mouseY * pivotSensitivity) * deltaTime;
        pivotAngle = Mathf.Clamp(pivotAngle, -40, 40);

        Vector3 rotation;
        Quaternion targetRotation;
        Quaternion currentRotation;
        Quaternion lerpRotation;

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        currentRotation = transform.rotation;
        lerpRotation = Quaternion.Slerp(currentRotation, targetRotation, cameraSpeed);
        transform.rotation = lerpRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        currentRotation = cameraPivot.localRotation;
        lerpRotation = Quaternion.Slerp(currentRotation, targetRotation, cameraSpeed);
        cameraPivot.localRotation = lerpRotation;

        
    }
}