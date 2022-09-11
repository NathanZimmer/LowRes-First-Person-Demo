using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 300;
    private Camera mainCamera;
    private float yRotation = 0;
    private bool isPaused = false;


    private void Start()
    {
        Application.targetFrameRate = 60; 
        mainCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        if (Input.GetAxis("Cancel") == 1)
        {
            isPaused = !isPaused;
            Cursor.lockState = (isPaused) ? CursorLockMode.None : CursorLockMode.Locked;
        }

        if (!isPaused)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);
            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation, -90, 90);
            mainCamera.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);
        }
    }
}