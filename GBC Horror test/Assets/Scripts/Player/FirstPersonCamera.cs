using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 300;
    [SerializeField] [Range(20, 120)] private int fov = 90;
    
    private Camera mainCamera;
    [HideInInspector] public float yRotation = 0;
    private bool isPaused = false;


    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        mainCamera.fieldOfView = fov;
        Cursor.lockState = CursorLockMode.Locked;
        Screen.SetResolution(1280, 720, true);
    }
    
    private void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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