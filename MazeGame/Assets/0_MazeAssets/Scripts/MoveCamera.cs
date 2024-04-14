using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;

    /** Camera logic to move with the player. */
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
