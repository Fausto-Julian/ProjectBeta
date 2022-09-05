using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraModel cameraModel;
    private InputActionAsset inputAsset;
    private InputActionMap playerControls;
    private bool isCameraLocked;

    private void Awake()
    {
        cameraModel = GetComponent<CameraModel>();
        inputAsset = this.GetComponentInParent<PlayerInput>().actions;
       
        playerControls = inputAsset.FindActionMap("PlayerControls");
    }
    private void Update()
    {
        cameraModel.CameraMode(Mouse.current.position.ReadValue(), isCameraLocked);
    }

    private void OnEnable()
    {
        playerControls.FindAction("CameraLock").performed += CameraLock;
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.FindAction("CameraLock").performed -= CameraLock;
        playerControls.Disable();
    }

    private void CameraLock(InputAction.CallbackContext obj)
    {
        isCameraLocked = !isCameraLocked;
    }
}
