using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Movement")]
    [Range(0.01f, 1.0f)]
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float threshold = 4.0f;

    // Internal variabled for camera movement
    private Vector3 offset; // Offset between the camera and the player
    private float fixedZ; // To store and maintain the camera's Z position
    // Internal variable for SmoothDamp function
    private Vector3 velocity = Vector3.zero;
    // Internal variables for platform snapping
    private Transform groundCheck;
    private Vector2 groundCheckSize;
    private LayerMask groundLayer;

    void Start()
    {
        float yPos = transform.position.y - player.transform.position.y;
        offset = new Vector3(0, yPos, 0);

        fixedZ = transform.position.z; // Store the initial Z position of the camera

        groundCheck = player.getGroundCheck();
        groundCheckSize = player.getGroundCheckSize();
        groundLayer = player.getGroundLayer();
    }

    void LateUpdate()
    {
        Vector3 newPosition = player.transform.position + offset;

        // Move the y_pos only if it is greater than threshold
        if (Math.Abs(newPosition.y - transform.position.y) < threshold)
        {
            newPosition.y = transform.position.y;
        }

        // Platform snapping
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer))
        {
            newPosition = player.transform.position + offset;
        }

        newPosition = new Vector3(newPosition.x, newPosition.y, fixedZ); // Maintain the fixed Z position
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }
}
