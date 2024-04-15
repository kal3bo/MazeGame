using UnityEngine;
using UnityEngine.InputSystem;

public class TorchBobbing : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovementScript; // Reference to the PlayerMovement script
    [SerializeField] private float bobbingSpeed = 0.18f; // Speed of the bobbing effect
    [SerializeField] private float bobbingAmount = 0.05f; // Amount of bobbing up and down

    private float timer = 0.0f;
    private Vector3 restPosition; // Initial position of the torch

    /** Store the initial local position of the torch. */
    void Start()
    {
        restPosition = transform.localPosition;
    }

    void Update()
    {
        Vector2 moveInput = playerMovementScript.moveInput;

        if (moveInput.magnitude > 0)
        {
            // Calculate the total movement to simulate bobbing
            float waveslice = Mathf.Sin(timer);
            timer += bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer -= Mathf.PI * 2;
            }

            // Modify vertical position based on the sin of the timer
            float translateChange = waveslice * bobbingAmount;
            Vector3 currentPosition = transform.localPosition;
            currentPosition.y = restPosition.y + translateChange;
            transform.localPosition = currentPosition;
        }
        else
        {
            // Reset the timer and smoothly return the torch to the resting position
            timer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, restPosition, Time.deltaTime * bobbingSpeed);
        }
    }
}
