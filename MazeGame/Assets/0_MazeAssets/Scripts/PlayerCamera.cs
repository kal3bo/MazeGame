using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float sensibilityXAxis = 100;
    [SerializeField] private float sensibilityYAxis = 100;

    [SerializeField] Transform cameraOrientation;

    private float xRotation;
    private float yRotation;

    // Input System:
    private PlayerInputActions inputActions;
    private Vector2 lookInput;

    /** Creates Input Action Bindings. */
    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    /** Enables Input System. */
    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    /** Disables Input System. */
    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    /** Locking the Mouse and not visible when the game starts. */
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /** Update is called once per frame. */
    void Update()
    {
        RotateCamera();
    }

    /** Moving the camera with Look Input. */
    private void RotateCamera()
    {
        float lookX = lookInput.x * sensibilityXAxis * Time.fixedDeltaTime;
        float lookY = lookInput.y * sensibilityYAxis * Time.fixedDeltaTime;

        yRotation += lookX;
        xRotation -= lookY;

        // Constraints:
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        // Apply Rotation:
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
