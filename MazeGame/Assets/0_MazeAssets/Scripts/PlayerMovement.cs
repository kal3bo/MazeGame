using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 moveInput;

    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private AudioSource footStepsSFX; // Serialized AudioSource field

    private Vector3 moveDirection;
    private Rigidbody playerBody;

    // Input System:
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
            ManageFootsteps(true);
        };
        inputActions.Player.Move.canceled += ctx =>
        {
            moveInput = Vector2.zero;
            ManageFootsteps(false);
        };
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerBody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = playerOrientation.forward * moveInput.y + playerOrientation.right * moveInput.x;
        if (moveDirection.magnitude > 0)
        {
            playerBody.AddForce(moveDirection.normalized * movementSpeed, ForceMode.Force);
        }
    }

    /** Manages the playing and stopping of footstep sounds */
    private void ManageFootsteps(bool isMoving)
    {
        if (isMoving && !footStepsSFX.isPlaying)
        {
            footStepsSFX.Play();
        }
        else if (!isMoving && footStepsSFX.isPlaying)
        {
            footStepsSFX.Stop();
        }
    }
}
