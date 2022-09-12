using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    private CameraModel cameraModel;
    private InputActionMap playerControls;
    private bool isCameraLocked;

    private void Awake()
    {
        cameraModel = GetComponent<CameraModel>();

       
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
        Debug.Log("espacio");
        isCameraLocked = !isCameraLocked;
    }
}
