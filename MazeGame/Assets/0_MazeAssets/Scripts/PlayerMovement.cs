using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 moveInput;

    [SerializeField] float movementSpeed;
    [SerializeField] Transform playerOrientation;

    private Vector3 moveDirection;

    private Rigidbody playerBody;

    // Input System:
    private PlayerInputActions inputActions;

    /** Creates Input Action Bindings. */
    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
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

    /** Getting RigidBody and Freezing Rotation. */
    private void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerBody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();    
    }

    /** Movement Logic using Input System. */
    private void MovePlayer()
    {
        moveDirection = playerOrientation.forward * moveInput.y + playerOrientation.right * moveInput.x;
        playerBody.AddForce(moveDirection.normalized * movementSpeed, ForceMode.Force); 
    }
}
